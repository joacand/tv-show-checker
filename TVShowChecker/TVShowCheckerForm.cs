using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace TVShowChecker
{
    public partial class TVShowCheckerForm : Form
    {
        private List<String> subscribedTVShows = new List<string>();
        private List<TVShow> TVShows = new List<TVShow>();
        private string subTVFile = @"SubscribedTV.xml";
        private string tvmazeAPIURL = @"http://api.tvmaze.com/search/shows?q=";

        public TVShowCheckerForm()
        {
            InitializeComponent();
            ReadSubscribedTVShows();
            CheckTV();
        }

        private void ReadSubscribedTVShows()
        {
            if (File.Exists(subTVFile))
            {
                using (var sr = new StreamReader(subTVFile))
                {
                    var serializer = new XmlSerializer(typeof(List<string>));
                    subscribedTVShows = serializer.Deserialize(sr) as List<string>;
                }
            }
        }

        private async void CheckTV()
        {
            await FillTVList();

            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            TVShows = TVShows.Distinct().ToList();

            foreach (TVShow show in TVShows)
            {
                dataGridView1.Rows.Add(show.Name, show.CurrentEpisodeNumber,
                    show.GetPreviousEpisodeTime(), show.GetTimeLeftForNextEpisode());
            }

            AutoAdjustWidths();
            FitFormToTable();
            SortTableByLatestEpisode();

            string pluralCharacter = TVShows.Count == 1 ? "" : "s";
            SetStatus($"Info available for {TVShows.Count} TV show{pluralCharacter}. Total: {subscribedTVShows.Count}.");
        }


        private void AutoAdjustWidths()
        {
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }
        }

        private void FitFormToTable()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                var height = dataGridView1.Rows[0].Height - 1;
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    height += dr.Height;
                }
                Height = height + 100;
            }
        }

        private void SortTableByLatestEpisode()
        {
            dataGridView1.SortCompare += CustomSortCompare;
            dataGridView1.Sort(dataGridView1.Columns[2], ListSortDirection.Ascending);
        }

        private async Task FillTVList()
        {
            string[] apiRequests = GetApiRequests();
            var showInfoJson = new List<string>(await GetMultipleAPIJson(apiRequests));

            var taskList = new List<Task<EpisodeInfo>>();
            for (int i = 0; i < subscribedTVShows.Count; i++)
            {
                taskList.Add(CreateEpisode(showInfoJson[i]));
            }

            var episodes = await Task.WhenAll(taskList.ToArray());
            var episodeInfos = new List<EpisodeInfo>(episodes);

            AddInformationToTvShowList(episodeInfos);
        }

        private async Task<EpisodeInfo> CreateEpisode(string showInfoJson)
        {
            var fullObject = JsonConvert.DeserializeObject<dynamic>(showInfoJson);
            if (fullObject.First == null)
            {
                return null;
            }
            string showName = fullObject.First.show.name;
            string nextEpHref = fullObject.First.show._links?.nextepisode?.href;
            string prevEpHref = fullObject.First.show._links?.previousepisode?.href;

            var nextEp = await GetEpisodeInfoJson(nextEpHref);
            var prevEp = await GetEpisodeInfoJson(prevEpHref);

            var nextEpRaw = GenPrevEpisodeInfo(showName, nextEp);
            var prevEpRaw = GenPrevEpisodeInfo(showName, prevEp);
            return new EpisodeInfo(showName, nextEpRaw, prevEpRaw);
        }

        private string[] GetApiRequests()
        {
            string[] apiRequests = new string[subscribedTVShows.Count];
            for (int i = 0; i < subscribedTVShows.Count; i++)
            {
                apiRequests[i] = tvmazeAPIURL + subscribedTVShows[i];
            }
            return apiRequests;
        }

        private async Task<string> GetEpisodeInfoJson(string apiRequest)
        {
            return await GetAPIJson(apiRequest);
        }

        private RawTVShow GenPrevEpisodeInfo(string name, string prevEpisodeJson)
        {
            if (string.IsNullOrWhiteSpace(prevEpisodeJson))
            {
                return null;
            }

            var fullObject = JsonConvert.DeserializeObject<dynamic>(prevEpisodeJson);

            string airDate = fullObject.airdate;
            string season = fullObject.season;
            season = season.PadLeft(2, '0');
            string number = fullObject.number;
            number = number.PadLeft(2, '0');

            return new RawTVShow(name, airDate, "S" + season + "E" + number);
        }

        private void AddInformationToTvShowList(List<EpisodeInfo> episodesInfos)
        {
            TVShows.Clear();

            foreach (var episode in episodesInfos)
            {
                TVShow newTV = new TVShow(episode.EpisodeName, episode?.PrevEp?.Episode, episode?.NextEp?.AirDate, episode?.PrevEp?.AirDate);
                TVShows.Add(newTV);
            }
        }

        private async Task<string[]> GetMultipleAPIJson(string[] apiRequests)
        {
            List<string> res = new List<string>();
            List<Task<string>> tasks = new List<Task<string>>();

            foreach (string req in apiRequests)
            {
                tasks.Add(GetAPIJson(req));
            }
            foreach (Task<string> task in tasks)
            {
                res.Add(await task);
            }
            return res.ToArray();
        }

        private async Task<string> GetAPIJson(string apiRequest)
        {
            if (apiRequest == null)
            {
                return null;
            }

            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetStringAsync(apiRequest);
            }
        }

        private void AddTvButton_Click(object sender, EventArgs e)
        {
            string newTV;
            AddTVDialog dialog = new AddTVDialog();
            dialog.Go(Location);
            string result = dialog.StatusMsgCallback.Trim();
            if (!string.IsNullOrWhiteSpace(result))
            {
                newTV = dialog.StatusMsgCallback;
                if (subscribedTVShows.Contains(newTV))
                {
                    SetStatus(newTV + " already exists.");
                }
                else
                {
                    subscribedTVShows.Add(newTV);
                    SaveTVShows();
                    CheckTV();
                    SetStatus("Added " + newTV + ".");
                }
            }
        }

        private void SaveTVShows()
        {
            using (StreamWriter sw = new StreamWriter(subTVFile))
            {
                var serializer = new XmlSerializer(subscribedTVShows.GetType());
                serializer.Serialize(sw, subscribedTVShows);
                sw.Flush();
            }
        }

        private void RmvTvButton_Click(object sender, EventArgs e)
        {
            int initialAmntElmts = subscribedTVShows.Count;
            RemoveTVDialog d = new RemoveTVDialog();
            d.Go(Location, subscribedTVShows);

            int removedElements = initialAmntElmts - subscribedTVShows.Count;
            if (removedElements > 0)
            {
                SaveTVShows();
                CheckTV();

                string pluralCharacter = removedElements == 1 ? "" : "s";
                SetStatus($"Removed {removedElements} TV show{pluralCharacter}.");
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            CheckTV();
            SetStatus("Refreshing.");
        }

        private void SetStatus(string statusText)
        {
            label_status.Text = statusText;
        }

        private void CustomSortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            string c1 = e.CellValue1.ToString().Split(' ')[0];
            string c2 = e.CellValue2.ToString().Split(' ')[0];
            if (c1.Equals(""))
            {
                e.SortResult = 1;
                e.Handled = true;
                return;
            }
            if (c2.Equals(""))
            {
                e.SortResult = -1;
                e.Handled = true;
                return;
            }
            if (!c1.IsNumeric() && !c2.IsNumeric())
            {
                e.SortResult = e.CellValue1.ToString().CompareTo(e.CellValue2.ToString());
                e.Handled = true;
                return;
            }
            if (!c1.IsNumeric())
            {
                e.SortResult = -1;
                e.Handled = true;
                return;
            }
            if (!c2.IsNumeric())
            {
                e.SortResult = 1;
                e.Handled = true;
                return;
            }

            int a = int.Parse(c1), b = int.Parse(c2);

            e.SortResult = a.CompareTo(b);

            e.Handled = true;
        }
    }
}
