namespace Hsr.Model.Models.ViewModel
{
    public class AreaVm
    {
        private State _state;

        public string id { get; set; }

        public string text { get; set; }

        public string pid { get; set; }

        public string icon { get; set; }

        public State state
        {
            get { return _state ?? (_state = new State()); }
            set { _state = value; }
        }

        //  public List<AreaVm> children { get; set; }

        public bool children { get; set; }

        public class State
        {
            public bool opened { get; set; }
            public bool disabled { get; set; }
            public bool selected { get; set; }
        }
    }
}