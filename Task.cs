using System;

namespace nm2
{
    public class Task
    {
        /*
        Класс должен иметь возможность
        1. Решать СЛАУ
        2. Поиск определителя матрицы (Гаус и декомпозиция)
        3. Поиск обратной матрицы

        Вопрос только в том как выбирать метод решения
        1. В файле доп. параметр
        2. В консоле пользователем (неудобно)
        UPD: Всё стало понятно Я(Иван) делаю Гауса, Дима доп и в файл можно вывести их оба

        Как решаем?
            Каждый метод сам в конце должен вывести в консоль и в файл необходимые данные (Гаус):
            а также
                При решении СЛАУ в файл выводятся:
                x – вектор решения;
                ε – вектор невязки;
                ||ε|| – норма вектора невязки.

                При поиске определителя – его значение.

                При вычислении обратной матрицы – следующие величины:
                X – обратная матрица;
                ε – матрица невязки (AX – E);
                ||ε|| – норма матрицы невязки.

         */

        private double[,] _taskMatrix; // Расширенная матрица системы 
        private uint _matrixRank; // Ранг той матрицы

        public Task(uint matrixRank, double[,] taskMatrix)
        {
            _matrixRank = matrixRank;
            _taskMatrix = new double[_matrixRank, _matrixRank + 1];
            for (int i = 0; i < matrixRank; i++)
            {
                for (int j = 0; j < matrixRank + 1; j++)
                {
                    _taskMatrix[i, j] = taskMatrix[i, j];
                }
            }
        }

        void SolveWithGauss(uint taskType)
        {
            double[,] locMatrix = (double[,])_taskMatrix.Clone();

            double[,] invertMatrix = new double[_matrixRank, _matrixRank]; /* Обратная матрица 
                                                                              Изначально единичная
            // f                                                                                  */ 
            for (int i = 0; i < _matrixRank; i++)
            {
                invertMatrix[i, i] = 1;
            }
            
            double det = 1; // Опредилитель матрицы системы

            
            
            // Пробегаем по строкам и находим коэфф. (множители 1-го шага, назову их 'm')
            for (int k = 0; k < _matrixRank; k++)
            {
                for (int i = k + 1; i < _matrixRank; i++)
                {
                    double m = locMatrix[i, k] / locMatrix[k, k]; /* множитель 1-го шага,
                                                                     будем умножать 1-ю строку на коэфф
                                                                     вычитать из каждой строки её */

                    for (int j = 0; j < _matrixRank + 1; j++)
                    {
                        locMatrix[i, j] -= locMatrix[k, j] * m;
                        
                    }
                }

                det *= locMatrix[k, k];
            }

            double[] resVector = new double[_matrixRank];

            double tmpColRes1, tmpColRes2;
            for (int i = (int)_matrixRank-1; i >= 0; i--)
            {   
                tmpColRes1 = 0;
                for (int j = i; j < _matrixRank; j++)
                {
                    tmpColRes2 = locMatrix[i, j] * resVector[j];
                    tmpColRes1 += tmpColRes2;
                }
                resVector[i] = (locMatrix[i,_matrixRank] - tmpColRes1) / locMatrix[i, i];
                
            }

            foreach (var val in resVector)
            {
                Console.Out.Write(val);
            }
        }

        public void Solve(uint taskType, string outFileName)
        {
            switch (taskType)
            {
                case 1:
                    SolveWithGauss(1);
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    throw new ArgumentException("Неверный тип задачи");
            }
        }
    }
}