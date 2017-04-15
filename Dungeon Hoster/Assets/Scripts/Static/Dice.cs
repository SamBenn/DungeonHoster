using System;

public static class Dice
{
    private static Random _rObj = new Random();

    private static int DBase(int max)
    {
        return _rObj.Next(1, max + 1);
    }

    public static int D2()
    {
        return DBase(2);
    }

    public static int D3()
    {
        return DBase(3);
    }

    public static int D4()
    {
        return DBase(4);
    }

    public static int D6()
    {
        return DBase(6);
    }

    public static int D8()
    {
        return DBase(8);
    }

    public static int D10()
    {
        return DBase(10);
    }

    public static int D20()
    {
        return DBase(20);
    }

    public static int D100()
    {
        return DBase(100);
    }
}