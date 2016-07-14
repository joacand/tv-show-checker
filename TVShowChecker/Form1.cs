using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace TVShowChecker {
    public partial class Form1 : Form {
        private List<String> subscribedTVShows = new List<string>();
        private List<TVShow> TVShows = new List<TVShow>();
        private string subTVFile = @"SubscribedTV.txt";
        private string tvmazeAPIURL = @"http://api.tvmaze.com/search/shows?q=";

        public Form1() {
            InitializeComponent();

            readSubscribedTVShows();
            checkTV();
        }

        private void readSubscribedTVShows() {
            if (File.Exists(subTVFile)) {
                using (StreamReader sr = new StreamReader(subTVFile)) {
                    string line;
                    while ((line = sr.ReadLine()) != null) {
                        subscribedTVShows.Add(line);
                    }
                }
            } else {
                using (var tmp = File.Create(subTVFile)) { };
            }
        }

        private async void checkTV() {
            await fillTVList();

            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            TVShows = TVShows.Distinct().ToList();

            foreach (TVShow show in TVShows) {
                dataGridView1.Rows.Add(show.name, show.currEpisodeNumber,
                    show.getLatestEpisodeTime(), show.getTimeLeftForNextEpisode());
            }

            autoAdjustWidths();
            fitFormToTable();
            sortTableByLatestEpisode();

            if (TVShows.Count == 1)
                setStatus("Info available for " + TVShows.Count + " TV show. Total: " + subscribedTVShows.Count + ".");
            else
                setStatus("Info available for " + TVShows.Count + " TV shows. Total: " + subscribedTVShows.Count + ".");
        }


        private void autoAdjustWidths() {
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            for (int i = 1; i < dataGridView1.Columns.Count; i++) {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        private void fitFormToTable() {
            if (dataGridView1.Rows.Count > 0) {
                var height = dataGridView1.Rows[0].Height - 1;
                foreach (DataGridViewRow dr in dataGridView1.Rows) {
                    height += dr.Height;
                }
                this.Height = height + 100;
            }
        }

        private void sortTableByLatestEpisode() {
            dataGridView1.SortCompare += customSortCompare;
            dataGridView1.Sort(dataGridView1.Columns[2], ListSortDirection.Ascending);
        }

        private async Task fillTVList() {
            string[] apiRequests = getApiRequests();
            List<string> showInfoJson = new List<string>(await getAPIJsonAsync(apiRequests));

            // Fetch information about the next and previous episodes
            TupleList<string, string> nextApiReqs = new TupleList<string, string>();
            TupleList<string, string> prevApiReqs = new TupleList<string, string>();

            for (int i = 0; i < subscribedTVShows.Count; i++) {
                var fullObject = JsonConvert.DeserializeObject<dynamic>(showInfoJson[i]);
                if (fullObject.First == null) {
                    continue;
                }
                string showName = fullObject.First.show.name;
                var nextEp = fullObject.First.show._links.nextepisode;
                var prevEp = fullObject.First.show._links.previousepisode;
                if (nextEp != null) {
                    string nextEpHref = nextEp.href;
                    nextApiReqs.Add(showName, nextEpHref);
                }
                if (prevEp != null) {
                    string prevEpHref = prevEp.href;
                    prevApiReqs.Add(showName, prevEpHref);
                }
            }

            string[] nextEpisodeJson = await getEpisodeInfoJson(nextApiReqs);
            string[] prevEpisodeJson = await getEpisodeInfoJson(prevApiReqs);

            List<RawTVShow> prevEpisodeInfo = genPrevEpisodeInfo(prevEpisodeJson, prevApiReqs);
            List<RawTVShow> nextEpisodeInfo = genNextEpisodeInfo(nextEpisodeJson, nextApiReqs);

            addInformationToTvShowList(prevEpisodeInfo, nextEpisodeInfo);
        }

        private string[] getApiRequests() {
            string[] apiRequests = new string[subscribedTVShows.Count];
            for (int i = 0; i < subscribedTVShows.Count; i++) {
                apiRequests[i] = tvmazeAPIURL + subscribedTVShows[i];
            }
            return apiRequests;
        }

        private async Task<string[]> getEpisodeInfoJson(TupleList<string, string> apiReqs) {
            List<string> apiReqsTemp = new List<string>();
            foreach (Tuple<string, string> t in apiReqs) {
                apiReqsTemp.Add(t.Item2);
            }
            string[] episodeInfoJson = await getAPIJsonAsync(apiReqsTemp.ToArray());
            return episodeInfoJson;
        }

        private List<RawTVShow> genNextEpisodeInfo(string[] nextEpisodeJson, TupleList<string, string> nextApiReqs) {
            List<string> nextEpisodeInfoTemp = parseNextEpisodeInformation(nextEpisodeJson);

            List<RawTVShow> nextEpisodeInfo = new List<RawTVShow>();
            for (int i = 0; i < nextApiReqs.Count; i++) {
                Tuple<string, string> nextApiReq = nextApiReqs[i];
                nextEpisodeInfo.Add(new RawTVShow(nextApiReq.Item1, nextEpisodeInfoTemp[i]));
            }

            return nextEpisodeInfo;
        }

        private List<string> parseNextEpisodeInformation(string[] nextEpisodeJson) {
            List<string> nextEpisodeInfo = new List<string>();
            for (int i = 0; i < nextEpisodeJson.Length; i++) {
                var fullObject = JsonConvert.DeserializeObject<dynamic>(nextEpisodeJson[i]);
                string airDate = fullObject.airdate;
                nextEpisodeInfo.Add(airDate);
            }
            return nextEpisodeInfo;
        }

        private List<RawTVShow> genPrevEpisodeInfo(string[] prevEpisodeJson, TupleList<string, string> prevApiReqs) {
            List<Tuple<string, string>> prevEpisodeInfoTemp = parsePrevEpisodeInformation(prevEpisodeJson);

            List<RawTVShow> prevEpisodeInfo = new List<RawTVShow>();
            for (int i = 0; i < prevApiReqs.Count; i++) {
                Tuple<string, string> prevApiReq = prevApiReqs[i];
                prevEpisodeInfo.Add(new RawTVShow(prevApiReq.Item1, prevEpisodeInfoTemp[i].Item1, prevEpisodeInfoTemp[i].Item2));
            }

            return prevEpisodeInfo;
        }

        private List<Tuple<string, string>> parsePrevEpisodeInformation(string[] prevEpisodeJson) {
            List<Tuple<string, string>> prevEpisodeInfo = new List<Tuple<string, string>>();
            for (int i = 0; i < prevEpisodeJson.Length; i++) {
                var fullObject = JsonConvert.DeserializeObject<dynamic>(prevEpisodeJson[i]);

                string airDate = fullObject.airdate;
                string season = fullObject.season;
                season = season.PadLeft(2, '0');
                string number = fullObject.number;
                number = number.PadLeft(2, '0');

                prevEpisodeInfo.Add(new Tuple<string, string>(airDate, "S" + season + "E" + number));
            }
            return prevEpisodeInfo;
        }

        private void addInformationToTvShowList(List<RawTVShow> prevEpisodeInfo, List<RawTVShow> nextEpisodeInfo) {
            TVShows.Clear();
            foreach (RawTVShow rawTvShow in prevEpisodeInfo) {
                TVShow newTV = new TVShow();
                newTV.name = rawTvShow.name;
                newTV.nextEpisode = "";
                foreach (RawTVShow nextShow in nextEpisodeInfo) {
                    if (nextShow.name.Equals(newTV.name)) {
                        newTV.nextEpisode = nextShow.airDate;
                    }
                }
                newTV.currEpisodeNumber = rawTvShow.episode;
                newTV.latestEpisode = rawTvShow.airDate;

                TVShows.Add(newTV);
            }
        }

        private async Task<string[]> getAPIJsonAsync(string[] apiRequests) {
            var client = new HttpClient();
            List<string> res = new List<string>();
            List<Task<string>> tasks = new List<Task<string>>();

            foreach (string req in apiRequests) {
                tasks.Add(ProcessURLAsync(req, client));
            }
            foreach (Task<string> task in tasks) {
                res.Add(await task);
            }

            return res.ToArray();
        }

        private async Task<string> ProcessURLAsync(string url, HttpClient httpClient) {
            return await httpClient.GetStringAsync(url);
        }

        private void button_addTV_Click(object sender, EventArgs e) {
            string newTV;
            AddTVDialog dialog = new AddTVDialog();
            dialog.go(this.Location);
            string result = dialog.getStatusMsgCallback().Trim();
            if (!result.Equals("")) {
                newTV = dialog.getStatusMsgCallback();
                if (subscribedTVShows.Contains(newTV)) {
                    setStatus(newTV + " already exists.");
                } else {
                    subscribedTVShows.Add(newTV);
                    saveTVShows();
                    checkTV();
                    setStatus("Added " + newTV + ".");
                }
            }
        }

        private void saveTVShows() {
            using (StreamWriter sw = new StreamWriter(subTVFile)) {
                foreach (string tvShow in subscribedTVShows) {
                    sw.WriteLine(tvShow);
                }
            }
        }

        private void button_rmvTvShow_Click(object sender, EventArgs e) {
            int initialAmntElmts = subscribedTVShows.Count;
            RemoveTVDialog d = new RemoveTVDialog();
            d.go(this.Location, subscribedTVShows); // Passing a reference to the list

            int removedElements = initialAmntElmts - subscribedTVShows.Count;
            if (removedElements > 0) {
                saveTVShows();
                checkTV();
                if (removedElements == 1)
                    setStatus("Removed " + removedElements + " TV show.");
                else
                    setStatus("Removed " + removedElements + " TV shows.");
            }
        }

        private void button_refresh_Click(object sender, EventArgs e) {
            checkTV();
            setStatus("Refreshing.");
        }

        private void setStatus(string s) {
            label_status.Text = s;
        }

        private class TupleList<T1, T2> : List<Tuple<T1, T2>> {
            public void Add(T1 item, T2 item2) {
                Add(new Tuple<T1, T2>(item, item2));
            }
        }

        private class RawTVShow {
            public string name { get; set; }
            public string airDate { get; set; }
            public string episode { get; set; }

            public RawTVShow(string n, string a, string e) {
                name = n;
                airDate = a;
                episode = e;
            }

            public RawTVShow(string n, string a) {
                name = n;
                airDate = a;
            }
        }

        private void customSortCompare(object sender, DataGridViewSortCompareEventArgs e) {
            string c1 = e.CellValue1.ToString().Split(' ')[0];
            string c2 = e.CellValue2.ToString().Split(' ')[0];
            if (c1.Equals("")) {
                e.SortResult = 1;
                e.Handled = true;
                return;
            }
            if (c2.Equals("")) {
                e.SortResult = -1;
                e.Handled = true;
                return;
            }
            if (!isNum(c1) && !isNum(c2)) {
                e.SortResult = e.CellValue1.ToString().CompareTo(e.CellValue2.ToString());
                e.Handled = true;
                return;
            }
            if (!isNum(c1)) {
                e.SortResult = -1;
                e.Handled = true;
                return;
            }
            if (!isNum(c2)) {
                e.SortResult = 1;
                e.Handled = true;
                return;
            }

            int a = int.Parse(c1), b = int.Parse(c2);

            e.SortResult = a.CompareTo(b);

            e.Handled = true;
        }

        private bool isNum(string a) {
            return a.All(char.IsDigit) && !a.Equals("");
        }
    }
}
