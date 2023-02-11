// Created by Mikhail S. Kataev (lpestl)
// Copyright © 2023 Mikhail S. Kataev. All rights reserved.

namespace PhotoOrganizer.Core;

public static class Helpers
{
    public static Tuple<double, string> GetSizeForUI(ulong sizeByte)
    {
        double size = (double)sizeByte;
        ushort unit = (ushort) ESizeUnit.Byte;
        while (size > 1024)
        {
            size = size / 1024;
            unit++;
        }

        return new Tuple<double, string>(size, ((ESizeUnit)unit).ToString());
    }
}