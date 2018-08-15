using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using TVShowChecker.Entities;
using TVShowChecker.Infrastructure;

namespace TVShowChecker
{
    public partial class TVShowCheckerForm : Form
    {
        private readonly List<String> subscribedTVShows;
        private readonly ITVShowService tvShowService;
        private readonly ConfigHandler configHandler = new ConfigHandler();

        public TVShowCheckerForm()
        {
            InitializeComponent();
            tvShowService = new TVMazeService();
            subscribedTVShows = configHandler.ReadSubscribedTvShowsFromConfig();
            RefreshTvList();
        }

        private async void RefreshTvList()
        {
            var tvShows = (await tvShowService.GetTvShows(subscribedTVShows)).Distinct().ToList();

            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            foreach (TVShow show in tvShows)
            {
                dataGridView1.Rows.Add(show.Name, show.CurrentEpisodeNumber,
                    show.GetPreviousEpisodeTime(), show.GetTimeLeftForNextEpisode());
            }

            AutoAdjustWidths();
            FitFormToTable();
            SortTableByLatestEpisode();

            string pluralCharacter = tvShows.Count == 1 ? "" : "s";
            SetStatus($"Info available for {tvShows.Count} TV show{pluralCharacter}. Total: {subscribedTVShows.Count}.");
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
                    SetStatus($"{newTV} already exists.");
                }
                else
                {
                    subscribedTVShows.Add(newTV);
                    SaveTVShows();
                    RefreshTvList();
                    SetStatus($"Added {newTV}.");
                }
            }
        }

        private void RmvTvButton_Click(object sender, EventArgs e)
        {
            int initialAmntElmts = subscribedTVShows.Count;
            RemoveTVDialog removeTvDialog = new RemoveTVDialog();
            removeTvDialog.Go(Location, subscribedTVShows);

            int removedElements = initialAmntElmts - subscribedTVShows.Count;
            if (removedElements > 0)
            {
                SaveTVShows();
                RefreshTvList();

                string pluralCharacter = removedElements == 1 ? "" : "s";
                SetStatus($"Removed {removedElements} TV show{pluralCharacter}.");
            }
        }

        private void SaveTVShows()
        {
            configHandler.SaveTvShowsToConfig(subscribedTVShows);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            SetStatus("Refreshing.");
            RefreshTvList();
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
