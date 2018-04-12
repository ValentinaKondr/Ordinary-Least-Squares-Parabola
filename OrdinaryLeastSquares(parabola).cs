using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdinaryLeastSquares
{
    class Program
    {
        public static double[] Gauss(double[,] a, double[] y)
        {

            double max;
            int k, index;
            const double eps = 0.00001;
            double[] coef = new double[3]; // a0, a1, a2 
            k = 0;
            while (k < 3)
            {
                // Поиск строки с максимальным элементом в первом столбце
                max = Math.Abs(a[k, k]);
                index = k;
                for (int i = k + 1; i < 3; i++)
                {
                    if (Math.Abs(a[i, k]) > max)
                    {
                        max = Math.Abs(a[i, k]);
                        index = i;
                    }
                }

                // Перестановка строк 

                for (int j = 0; j < 3; j++)
                {
                    double temp = a[k, j];
                    a[k, j] = a[index, j];
                    a[index, j] = temp;
                }

                // Перестановка столбцов с ответами
                double tmp = y[k];
                y[k] = y[index];
                y[index] = tmp;

                for (int i = k; i < 3; i++)
                {
                    double temp = a[i, k];
                    if (Math.Abs(temp) < eps)
                        continue; // для нулевого коэффициента пропустить 
                    for (int j = 0; j < 3; j++)
                        a[i, j] = a[i, j] / temp;
                    y[i] = y[i] / temp;
                    if (i == k) continue; // уравнение не вычитать само из себя 
                    for (int j = 0; j < 3; j++)
                        a[i, j] = a[i, j] - a[k, j];
                    y[i] = y[i] - y[k];
                }
                k++;
            }


            // Обратный ход 
            for (k = 3 - 1; k >= 0; k--)
            {
                coef[k] = y[k];
                for (int i = 0; i < k; i++)
                    y[i] = y[i] - a[i, k] * coef[k];
            }
            return coef;
        }




        static void Main(string[] args)
        {

            Console.Title = "Метод наименьших квадратов";
            int n = 10;
            bool success;   //Для ввода с клавиатуры
            //--------Ввод размерности
            /*            
                        do
                        {
                            Console.WriteLine("Введите количество точек, которые необходимо аппроксимировать");
                            success = Int32.TryParse(Console.ReadLine(), out n);
                            if (!success)
                            {
                                Console.WriteLine("Ошибка ввода! Это должно быть целое неотрицательное число");
                            }
                            else if (n <= 0)
                            {
                                Console.WriteLine("Ошибка ввода! Это должно быть целое неотрицательное число");
                                success = false;
                            }
                        } while (!success);
             */
            //--------

            //Быстрый ввод 10 точек
            double[] xCoordinates = new double[] { 4.302, 4.381, 4.626, 4.886, 4.808, 4.872, 4.382, 4.181, 4.483, 4.418 };
            double[] yCoordinates = new double[] { 5.496, 5.645, 6.894, 8.175, 7.738, 8.272, 5.567, 4.883, 6.175, 5.681 };

            int i = 0;
            /*            
                        //-------Ввод данных в массивы с исключением ошибок
                        while (i < n)
                        {
                            Console.WriteLine("Точка номер " + (i+1));
                            Console.WriteLine("Введите координату X: ");
                
                            success = Double.TryParse(Console.ReadLine(), out xCoordinates[i]);
                            if (!success)
                            {
                                Console.WriteLine("Ошибка ввода! Это должно быть целое неотрицательное число");
                            } else
                            {
                                Console.WriteLine("Введите координату Y: ");
                                success = Double.TryParse(Console.ReadLine(), out yCoordinates[i]);
                                    if (!success)
                                    {
                                        Console.WriteLine("Ошибка ввода! Это должно быть целое неотрицательное число");
                                    } else {i++;}
                             }
                        }
            
                        */
            //-------Вывод всего введенного на экран
            Console.WriteLine("Введенные координаты: ");
            Console.Write("X|  ");
            foreach (double d in xCoordinates)
                Console.Write("{0,4} ", d);

            Console.Write("\nY|  ");
            foreach (double d in yCoordinates)
                Console.Write("{0,4} ", d);
            Console.WriteLine();
            //-------

            double sumX = 0;        //SIGMA x
            double sumXX = 0;       //SIGMA x^2
            double sumXXX = 0;      //SIGMA x^3
            double sumXXXX = 0;     //SIGMA x^4
            double sumY = 0;        //SIGMA y
            double sumXY = 0;       //SIGMA x*y
            double sumXXY = 0;      //SIGMA (x^2)*y

            //-------

            for (i = 0; i < n; i++)
            {
                sumX += xCoordinates[i];
                sumY += yCoordinates[i];
                sumXY += xCoordinates[i] * yCoordinates[i];
                sumXX += xCoordinates[i] * xCoordinates[i];
                sumXXX += Math.Pow(xCoordinates[i], 3);
                sumXXXX += Math.Pow(xCoordinates[i], 4);
                sumXXY += Math.Pow(xCoordinates[i], 2) * yCoordinates[i];
            }

            double err = 0; // Оценка погрешности

            double[,] Matrix = new double[3, 3]
                {   
                    { 10, sumX, sumXX },
                    { sumX, sumXX, sumXXX },
                    { sumXX, sumXXX, sumXXXX}  
                };
            double[] Col = new double[3] { sumY, sumXY, sumXXY };
            double[] ans = new double[3];
            ans = Gauss(Matrix, Col);

            for (i = 0; i < n; i++)
            {
                err += Math.Pow((yCoordinates[i] - (ans[0] + ans[1] * xCoordinates[i] + ans[2] * xCoordinates[i] * xCoordinates[i])), 2);
            }


            Console.WriteLine("Полученное уравнение: y={0:f4}+({1:f4}x)+({2:f4}x*x)", ans[0], ans[1], ans[2]);
            Console.WriteLine("Сумма квадратов погрешностей: {0}", err);
            Console.Read();
        }
    }
}
