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
        
        Task(uint matrixRank, double[,] taskMatrix)
        {
            _matrixRank = matrixRank;
            _taskMatrix = new double[_matrixRank, _matrixRank + 1];
            for (int i = 0; i < matrixRank; i++)
            {
                for (int j = 0; j < matrixRank+1; j++)
                {
                    _taskMatrix[i, j] = taskMatrix[i, j];
                }
            }
        }

        void Solve(uint taskType, string outFileName)
        {
            switch (taskType)
            {
                case 1:
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