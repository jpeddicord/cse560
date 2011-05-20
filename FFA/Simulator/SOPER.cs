using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErrCat = Assembler.Errors.Category; 

namespace Simulator
{
    class SOPER
    {
        public static void Run(string function, string bin)
        {
            if (function == "ADD")
            {

            }
            else if (function == "SUB")
            {

            }
            else if (function == "MUL")
            {

            }
            else if (function == "DIV")
            {

            }
            else if (function == "OR")
            {

            }
            else if (function == "AND")
            {

            }
            else if (function == "READN")
            {

            }
            else if (function == "READC")
            {

            }
            else if (function == "WRITEN")
            {

            }
            else if (function == "WRITEC")
            {

            }
        }

        public static void Add(int n)
        {
            if (n > 1)
            {
                try
                {
                    Memory m = Memory.GetInstance();
                    int total = 0;
                    while (n > 0)
                    {
                        total = total + m.DataPop();
                    }

                    if (total > 65535)
                    {
                        throw new Assembler.ErrorException(ErrCat.Serious, 11);
                    }

                    m.DataPush(total);
                }
                catch (Exception)
                {
                    // TODO Handle error nothing on data stack
                }
            }
        }

        public static void Sub(int n)
        {
            if (n > 1)
            {
                try
                {
                    Memory m = Memory.GetInstance();
                    int total = m.DataPop();

                    while (n > 1)
                    {
                        total = total - m.DataPop();
                    }

                    if (total < 0)
                    {
                        throw new Assembler.ErrorException(ErrCat.Serious, 11);
                    }

                    m.DataPush(total);

                }
                catch (Exception)
                {
                    // TODO Handle error nothing on data stack
                }
            }
        }

        public static void Mul(int n)
        {
            if (n > 1)
            {
                try
                {
                    Memory m = Memory.GetInstance();
                    int total = 1;
                    while (n > 0)
                    {
                        total = total * m.DataPop();
                    }

                    if (total > 65535)
                    {
                        throw new Assembler.ErrorException(ErrCat.Serious, 11);
                    }

                    m.DataPush(total);
                }
                catch (Exception)
                {
                    // TODO Handle error nothing on data stack
                }
            }
        }

        public static void Div(int n)
        {
            if (n > 1)
            {
                try
                {
                    Memory m = Memory.GetInstance();
                    int total = m.DataPop();

                    while (n > 1)
                    {
                        int i = m.DataPop();

                        if (i == 0)
                        {
                            throw new Assembler.ErrorException(ErrCat.Serious, 12);
                        }

                        total = total / m.DataPop();
                    }

                    if (total > 65535)
                    {
                        throw new Assembler.ErrorException(ErrCat.Serious, 11);
                    }

                    m.DataPush(total);
                }
                catch (Exception)
                {
                    // TODO Handle error nothing on data stack
                }
            }
        }

        public static void Or(int n)
        {
            if (n > 1)
            {
                try
                {
                    Memory m = Memory.GetInstance();
                    int total = m.DataPop();

                    while (n > 1)
                    {
                        total = total | m.DataPop();
                    }

                    m.DataPush(total);

                }
                catch (Exception)
                {
                    // TODO Handle error nothing on data stack
                }
            }
        }

        public static void And(int n)
        {
            if (n > 1)
            {
                try
                {
                    Memory m = Memory.GetInstance();
                    int total = m.DataPop();

                    while (n > 1)
                    {
                        total = total & m.DataPop();
                    }

                    m.DataPush(total);

                }
                catch (Exception)
                {
                    // TODO Handle error nothing on data stack
                }
            }
        }
    }
}
