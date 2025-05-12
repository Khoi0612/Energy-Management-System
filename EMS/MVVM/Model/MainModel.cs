namespace EMS.MVVM.Model
{
    internal class MainModel
    {
        public MainModel()
        {
            CurrentModel = new CurrentModel();
            VoltageModel = new VoltageModel();
            PowerModel = new PowerModel();
        }

        // Model properties
        public CurrentModel CurrentModel { get; set; }
        public VoltageModel VoltageModel { get; set; }
        public PowerModel PowerModel { get; set; }
    }
}
