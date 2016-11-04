using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V电影.ViewModel
{
    public class BehindPageViewModel : BasePageViewModel
    {
        private ObservableCollection<Model.behind_data> _behind_data;
        public ObservableCollection<Model.behind_data> Behind_Data
        {
            get
            {
                try
                {
                    if (_behind_data != null)
                    {
                        for (int i = 0; i < _behind_data.Count; i++)
                        {
                            if (_behind_data[i].Image_Sbs != null)
                            {
                                for (int j = 0; j < _behind_data[i].Image_Sbs.Count; j++)
                                {
                                    if (j > _behind_data[i].Behind_Info.Count - 1)
                                    {
                                        break;
                                    }
                                    _behind_data[i].Behind_Info[j].image_source = _behind_data[i].Image_Sbs[j];
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
                return _behind_data;
            }
            set
            {
                _behind_data = value;
                RaisePropertyChanged(nameof(Behind_Data));
            }
        }
    }
}
