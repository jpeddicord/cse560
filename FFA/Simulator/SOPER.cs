using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ErrCat = Assembler.Errors.Category; 

namespace Simulator
{
    class SOPER
    {
        public SOPER()
        {
            // Logger?
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
                        total = total + Convert.ToInt32(m.DataPop(), 16);
                    }

                    if (total > 65535)
                    {
                        throw new Assembler.ErrorException(ErrCat.Serious, 11);
                    }

                    m.DataPush(Convert.ToString(total, 16));
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
                    int total = Convert.ToInt32(m.DataPop(), 16);

                    while (n > 1)
                    {
                        total = total - Convert.ToInt32(m.DataPop(), 16);
                    }

                    if (total < 0)
                    {
                        throw new Assembler.ErrorException(ErrCat.Serious, 11);
                    }

                    m.DataPush(Convert.ToString(total, 16));

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
                        total = total * Convert.ToInt32(m.DataPop(), 16);
                    }

                    if (total > 65535)
                    {
                        throw new Assembler.ErrorException(ErrCat.Serious, 11);
                    }

                    m.DataPush(Convert.ToString(total, 16));
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
                    int total = Convert.ToInt32(m.DataPop(), 16);

                    while (n > 1)
                    {
                        int i = Convert.ToInt32(m.DataPop(), 16);

                        if (i == 0)
                        {
                            throw new Assembler.ErrorException(ErrCat.Serious, 12);
                        }

                        total = total / Convert.ToInt32(m.DataPop(), 16);
                    }

                    if (total > 65535)
                    {
                        throw new Assembler.ErrorException(ErrCat.Serious, 11);
                    }

                    m.DataPush(Convert.ToString(total, 16));
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
                    int total = Convert.ToInt32(m.DataPop(), 16);

                    while (n > 1)
                    {
                        total = total | Convert.ToInt32(m.DataPop(), 16);
                    }

                    m.DataPush(Convert.ToString(total, 16));

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
                    int total = Convert.ToInt32(m.DataPop(), 16);

                    while (n > 1)
                    {
                        total = total & Convert.ToInt32(m.DataPop(), 16);
                    }

                    m.DataPush(Convert.ToString(total, 16));

                }
                catch (Exception)
                {
                    // TODO Handle error nothing on data stack
                }
            }
        }
    }
}
