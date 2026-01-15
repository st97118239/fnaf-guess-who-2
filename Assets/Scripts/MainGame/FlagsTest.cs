using System;

public class FlagsTest
{
    [Flags]
    public enum CanInteractWith
    {
        Nothing = 0,
        Refrigerator,
        Car,
        Airplane,
        Switch
    }

    public enum CanInteractWithNoFlags
    {
        Nothing = 0,
        Refrigerator = 1,
        Car = 2,
        Airplane = 3,
        Switch = 4,
        Max = 5
    }

    private bool[] _canInteractWithNoFlags = new bool[(int) CanInteractWithNoFlags.Max]; // 1 bool is 64 bit (8 bytes), en dus x 5 elementen is 40 bytes
    private CanInteractWith _canInteractWith = CanInteractWith.Nothing; // 1 byte

    public FlagsTest()
    {
        _canInteractWithNoFlags[(int)CanInteractWithNoFlags.Airplane] = true;
        _canInteractWithNoFlags[(int)CanInteractWithNoFlags.Car] = true;



        _canInteractWith = CanInteractWith.Airplane | CanInteractWith.Car; // 0000 1010
    }

    public bool CanInteractWithAirplaneNoFlags()
    {
        return _canInteractWithNoFlags[(int)CanInteractWithNoFlags.Airplane];
    }

    public bool CanInteractWithOther(CanInteractWith compareWith)
    {
        return (_canInteractWith & compareWith) == compareWith; // 100% match, als in alle flags in compareWith moeten matchen

        // return (_canInteractWith & compareWith) > 0; // 1 van de flags van compareWith moet matchen

    }

    public void AddCanInteractWith(CanInteractWith add)
    {
        _canInteractWith |= add;
    }

    public void RemoveCanInteractWith(CanInteractWith subtract)
    {
        _canInteractWith &= ~subtract;
    }
}

/*
 * 0000 1000
 * 0000 0010 |
 * 0000 1010
 *
 * ~(Airplane | Car)
 * 1111 0101
 * 0000 1010 &
 * 0000 0010
 *
 * 0000 1010
 * 0001 1000 // Airplane | Switch
 *           &
 * 0000 1000 == 4
 */
