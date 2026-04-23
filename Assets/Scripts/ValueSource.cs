[System.Serializable]
public class ValueSource
{
    public enum Mode
    {
        Constant,
        MissingHealth,
        StackCount,
        Custom,
        SecondsTransformed
    }

    public Mode mode;

    public float constant;
    public float multiplier;

    public float Evaluate()
    {
        switch (mode)
        {
            case Mode.Constant:
                return constant;

            case Mode.SecondsTransformed:
                return 1 + (GameManager.instance.player.timeTransformed * multiplier /100);


        }

        return 0;
    }
}