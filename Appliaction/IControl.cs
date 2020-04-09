namespace tmp
{
    public interface IControl
    {
        float MouseSensitivity { get; set; }
        void Move(float time);
        void MouseMove();
    }
}