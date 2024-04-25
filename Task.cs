using System;
using System.IO;
using System.Text;

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

        private Matrix _taskMatrix; // Расширенная матрица системы 
        private uint _matrixRank; // Ранг той матрицы

        public Task(uint matrixRank, Matrix taskMatrix)
        {
            _matrixRank = matrixRank;
            _taskMatrix = new Matrix(_matrixRank, _matrixRank + 1);
            for (uint i = 0; i < matrixRank; i++)
            {
                for (uint j = 0; j < matrixRank + 1; j++)
                {
                    _taskMatrix[i, j] = taskMatrix[i, j];
                }
            }
        }

        private Vector Residuals(Matrix taskMatrix,Vector ansVec) // Вектор невязки
        {
            var result = new Vector(ansVec.Size);
            for (uint i = 0; i < ansVec.Size; i++)
            {
                result[i] = taskMatrix[i, taskMatrix.Cols-1];
            }
            for (uint i = 0; i < taskMatrix.Rows; i++)
            {
                double temp = 0;
                for (uint j = 0; j < taskMatrix.Cols-1; j++)
                {
                    temp += taskMatrix[i,j] * ansVec[j];
                }
                result[i] -= temp;
            }
            return result;
        }
        

        private object SolveWithGauss(uint taskType, Matrix taskMatrix, bool triangleMatrix = false)
        {
            Matrix locMatrix = (Matrix)taskMatrix.Clone();

            Matrix invertMatrix = new Matrix(_matrixRank, _matrixRank); /* Обратная матрица
                                                                              Изначально единичная
            // f                                                                                  */
            for (uint i = 0; i < _matrixRank; i++)
            {
                invertMatrix[i, i] = 1;
            }

            double det = 1; // Опредилитель матрицы системы

            var sb = new StringBuilder();

            // Пробегаем по строкам и находим коэфф. (множители 1-го шага, назову их 'm')
            if (!triangleMatrix)
            {
                for (uint k = 0; k < _matrixRank; k++)
                {
                    for (uint i = k + 1; i < _matrixRank; i++)
                    {
                        double m = locMatrix[i, k] / locMatrix[k, k]; /* множитель 1-го шага,
                                                                     будем умножать 1-ю строку на коэфф
                                                                     вычитать из каждой строки её */

                        for (uint j = 0; j < _matrixRank; j++)
                        {
                            invertMatrix[i, j] -= invertMatrix[k, j] * m;
                            locMatrix[i, j] -= locMatrix[k, j] * m;
                        }

                        locMatrix[i, _matrixRank] -= locMatrix[k, _matrixRank] * m;
                    }

                    sb.AppendLine($"A{k + 1}");
                    // for (uint i = 0; i < _matrixRank; i++)
                    // {
                    //     for (uint j = 0; j < _matrixRank + 1; j++)
                    //     {
                    //         //sb.Append(locMatrix[i, j].ToString("\t 0.000;\t-0.000"));
                    //     }
                    //
                    //     //sb.AppendLine();
                    // }

                    sb.AppendLine(locMatrix.ToString());
                    det *= locMatrix[k, k];
                }

                sb.AppendLine();
            }

            Vector resVector = new Vector(_matrixRank);

            int cnt = 1;
            double tmpColRes1, tmpColRes2;
            for (int ii = (int)_matrixRank - 1; ii >= 0; ii--)
            {
                uint i = (uint)ii;
                tmpColRes1 = 0;
                for (uint j = i; j < _matrixRank; j++)
                {
                    tmpColRes2 = locMatrix[i, j] * resVector[j];
                    tmpColRes1 += tmpColRes2;
                }

                locMatrix[i, _matrixRank] = (locMatrix[i, _matrixRank] - tmpColRes1) / locMatrix[i, i];
                resVector[i] = locMatrix[i, _matrixRank];
                sb.AppendLine($"\nb{cnt++}");
                for (uint j = 0; j < _matrixRank; j++)
                {
                    sb.Append(locMatrix[j, _matrixRank].ToString("\t 0.000;\t-0.000"));
                }
            }

            sb.AppendLine();

            switch (taskType)
            {
                case 0:
                    return resVector;
                    
                case 1:
                    sb.AppendLine("Вектор решения:");
                    foreach (var val in resVector)
                    {
                        sb.Append(val + " ");
                    }
                    sb.AppendLine("\nВектор невязки:");
                    Vector residual = Residuals(taskMatrix, resVector);
                    foreach (var val in residual)
                    {
                        sb.Append(val + " ");
                    }

                    sb.AppendLine($"\nНорма вектора невязки:\n{residual.Norm()}");
                    
                    return sb;
                case 2:
                    sb.AppendLine($"\nОпределитель:\n{det}");
                    return sb;
                case 3:

                    var tmpInvertMatrix = (double[,])invertMatrix.Data.Clone();
                    for (uint i = 0; i < _matrixRank; i++)
                    {
                        sb.AppendLine($"e{i + 1}");
                        Matrix invTaskMatrix = locMatrix;
                        for (uint j = 0; j < _matrixRank; j++)
                        {
                            invTaskMatrix[j, _matrixRank] = tmpInvertMatrix[j, i];
                        }

                        var invVector = ((Vector)SolveWithGauss(0, invTaskMatrix, true)).Data;
                        for (uint j = 0; j < _matrixRank; j++)
                        {

                            sb.Append(invVector[j].ToString("\t 0.000; \t-0.000"));
                            invertMatrix[j, i] = invVector[j];
                        }

                        sb.AppendLine();
                    }

                    sb.AppendLine("Обратная матрица:");
                    sb.AppendLine(invertMatrix.ToString());
                    sb.AppendLine("Матрица невязки:");
                    var tmpMatrix = new Matrix(taskMatrix.Rows,taskMatrix.Rows);
                    for (uint i = 0; i < taskMatrix.Rows; i++)
                    {
                        for (uint j = 0; j < taskMatrix.Rows; j++)
                        {
                            tmpMatrix[i, j] = taskMatrix[i, j];
                        }
                    }
                    invertMatrix = tmpMatrix * invertMatrix;
                    var identetiMatrix = new Matrix(tmpMatrix.Rows, tmpMatrix.Rows);
                    for (uint i = 0; i < tmpMatrix.Rows; i++)
                    {
                        identetiMatrix[i, i] = 1;
                    }

                    invertMatrix -= identetiMatrix;
                    sb.AppendLine(invertMatrix.ToString());
                    sb.AppendLine($"Норма матрицы невязки:\n{invertMatrix.Norm()}");
                    
                    return sb.ToString();
            }

            return sb.ToString();
        }

        public void Solve(uint taskType, string outFileName)
        {
            if (taskType > 3)
                throw new ArgumentException("Неверный тип задачи");
            var outStr = SolveWithGauss(taskType, _taskMatrix);
            using (FileStream fstream = new FileStream(outFileName, FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] buffer = Encoding.Default.GetBytes((string)outStr);
                // запись массива байтов в файл
                fstream.WriteAsync(buffer, 0, buffer.Length);
            }
            Console.Out.WriteLine(outStr);
        }
    }
}