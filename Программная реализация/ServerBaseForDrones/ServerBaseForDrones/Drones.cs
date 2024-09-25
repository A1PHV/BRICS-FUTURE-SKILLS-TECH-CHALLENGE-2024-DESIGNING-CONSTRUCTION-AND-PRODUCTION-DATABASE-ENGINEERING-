namespace ServerBaseForDrones
{
    public class Drone
    {
        public int Id { get; set; }  // Порядковый номер дрона
        public string Status { get; set; }  // Состояние дрона (например, "в полёте", "заряжается", "ожидание")
    }
}
