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
                throw new NotImplementedException();
            }
            else if (function == "SUB")
            {
                throw new NotImplementedException();
            }
            else if (function == "MUL")
            {
                throw new NotImplementedException();
            }
            else if (function == "DIV")
            {
                throw new NotImplementedException();
            }
            else if (function == "OR")
            {
                throw new NotImplementedException();
            }
            else if (function == "AND")
            {
                throw new NotImplementedException();
            }
            else if (function == "READN")
            {
                throw new NotImplementedException();
            }
            else if (function == "READC")
            {
                throw new NotImplementedException();
            }
            else if (function == "WRITEN")
            {
                throw new NotImplementedException();
            }
            else if (function == "WRITEC")
            {
                throw new NotImplementedException();
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
