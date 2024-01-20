using UnityEngine;

public static class NumberParser
{
    public static string FromNumberToShortText(int number)
    {
        string numberReduction = "";

        string numberBeforeReduction = number.ToString();

        string numberAfterDot = "";

        int numberCount = numberBeforeReduction.Length;

        if (4 <= numberCount && numberCount <= 6)
        {
            numberBeforeReduction = numberBeforeReduction.Remove(numberCount - 2);

            numberAfterDot = "." + numberBeforeReduction[numberBeforeReduction.Length - 1].ToString();

            numberBeforeReduction = numberBeforeReduction.Remove(numberBeforeReduction.Length - 1);

            numberReduction = "K";
        }
        else if (numberCount > 6)
        {
            numberBeforeReduction = numberBeforeReduction.Remove(numberCount - 5);

            numberAfterDot = "." + numberBeforeReduction[numberBeforeReduction.Length - 1].ToString();

            numberBeforeReduction = numberBeforeReduction.Remove(numberBeforeReduction.Length - 1);

            numberReduction = "M";
        }

        return numberBeforeReduction + numberAfterDot + numberReduction;
    }

    public static string NumberToPositionText(int number)
    {
        string lastNum = number.ToString();

        lastNum = lastNum[lastNum.Length - 1].ToString();

        switch (lastNum)
        {
            case "1":
                lastNum = "ST";
                break;
            case "2":
                lastNum = "ND";
                break;
            case "3":
                lastNum = "RD";
                break;
            default:
                lastNum = "TH";
                break;
        }

        return number + lastNum;
    }
}
