namespace iDental.iDentalClass
{
    public class ComboBoxItemInfo
    {
        public string DisplayName { get; set; }
        public int SelectedValue { get; set; }

        public ComboBoxItemInfo() { }
        public ComboBoxItemInfo(string displayName, int selectedValue)
        {
            DisplayName = displayName;
            SelectedValue = selectedValue;
        }
    }
}
