namespace StudyControlWeb.ViewModels
{
    public enum SortState
    {
        FirstAsc,
        FirstDesc,
        SecondAsc,
        SecondDesc,
        ThirdAsc,
        ThirdDesc,
        FourthAsc,
        FourthDesc,
        FifthAsc,
        FifthDesc,
    }
    public class SortViewModel
    {
        public SortState FirstSort { get; private set; }
        public SortState SecondSort { get; private set; }
        public SortState ThirdSort { get; private set; }
        public SortState FourthSort { get; private set; }
        public SortState FifthSort { get; private set; }
        public SortState Current { get; private set; }

        public SortViewModel(SortState sortOrder)
        {
            FirstSort = sortOrder == SortState.FirstAsc ? SortState.FirstDesc : SortState.FirstAsc;
            SecondSort = sortOrder == SortState.SecondAsc ? SortState.SecondDesc : SortState.SecondAsc;
            ThirdSort = sortOrder == SortState.ThirdAsc ? SortState.ThirdDesc : SortState.ThirdAsc;
            FourthSort = sortOrder == SortState.FourthAsc ? SortState.FourthDesc : SortState.FourthAsc;
            FifthSort = sortOrder == SortState.FifthAsc ? SortState.FifthDesc : SortState.FifthAsc;
            Current = sortOrder;
        }
    }
}
