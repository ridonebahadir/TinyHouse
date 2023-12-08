namespace _Project.Scripts
{
    public interface IAreaElement
    {
        public string AreaID { get; set; }
        public AreaController AssignedArea { get; set; }
        public void InitArea(AreaController area);
    }
}