using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.ViewModel
{
    public class SeriesViewViewModel : BasePageViewModel
    {
        private Model.series_view _series_view;
        public Model.series_view Series_View
        {
            get
            {
                return _series_view;
            }
            set
            {
                _series_view = value;
                RaisePropertyChanged(nameof(Series_View));
            }
        }

        private Model.series_view_item _current_playing_item;
        public Model.series_view_item Current_Playing_Item
        {
            get
            {
                return _current_playing_item;
            }
            set
            {
                _current_playing_item = value;
                RaisePropertyChanged(nameof(Current_Playing_Item));
            }
        }

        private Model.series_video_info _current_video_info;
        public Model.series_video_info Current_Video_Info
        {
            get
            {
                return _current_video_info;
            }
            set
            {
                _current_video_info = value;
                RaisePropertyChanged(nameof(Current_Video_Info));
            }
        }
    }
}