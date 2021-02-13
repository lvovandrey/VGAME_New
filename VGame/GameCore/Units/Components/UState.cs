using VGameCore.Abstract;

namespace VGameCore.Units.Components
{
    public enum NewOld { New, Old }
    public enum Visibility { Empty, Collapse, Hidden, Visible }

    public class UState : Component
    {
        #region constructors
        public UState(string name, IComponentContainer container) : base(name, container)
        {
            newOld = NewOld.New;
            visibility = Visibility.Empty;
            IsHited = false;
        }
        public UState(string name, IComponentContainer container, NewOld newold = NewOld.New, Visibility visibility_ = Visibility.Empty, bool isHited = false) : this(name, container)
        {
            newOld = newold;
            visibility = visibility_;
            IsHited = isHited;
        }
        #endregion

        #region variables
        #endregion

        #region properties
        public NewOld newOld { get; set; }
        public Visibility visibility { get; set; }
        public bool IsHited { get; set; }
        #endregion

        #region methods
        #endregion

    }
}
