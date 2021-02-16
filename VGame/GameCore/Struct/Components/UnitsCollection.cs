using System;
using System.Linq;
using System.Collections.Generic;
using VGameCore.Units;
using VGameCore.Units.Components;
using System.Collections;
using VGameCore.Abstract;

namespace VGameCore.Struct.Components
{
    public class UnitsCollection<U> : Abstract.Component where U : Unit
    {


        #region constructors
        public UnitsCollection(string name, Abstract.IComponentContainer container) : base(name, container)
        {
            Units = new List<U>();
        }
        #endregion

        #region variables
        #endregion

        #region properties
        private List<U> Units;
        #endregion

        #region methods
        /// <summary>
        /// Возвращает все юниты в общей коллекции юнитов
        /// </summary>
        /// <returns>список всех юнитов</returns>
        public List<U> GetAllUnits()
        {
            return new List<U>(Units);
        }

        /// <summary>
        /// Возвращает список из "новых" элементов.   (переписать под Linq)
        /// </summary>
        /// <returns>Список новых элементов</returns>
        public List<U> GetNewUnits()
        {
            List<U> NewU = new List<U>();
            foreach (U u in Units)
            {
                if (u.GetComponent<UState>().newOld == NewOld.New)
                {
                    NewU.Add(u);
                }
            }
            return NewU;
        }

        /// <summary>
        /// Возвращает список из "старых" элементов.   (переписать под Linq)
        /// </summary>
        /// <returns>Список старых элементов</returns>
        public List<U> GetOldUnits()
        {
            List<U> NewU = new List<U>();
            foreach (U u in Units)
            {
                if (u.GetComponent<UState>().newOld == NewOld.Old)
                {
                    NewU.Add(u);
                }
            }
            return NewU;
        }

        /// <summary>
        /// Возвращает true если все элементы "старые"
        /// </summary>
        /// <returns>Возвращает true если все элементы "старые"</returns>
        public bool IsAllUnitsOld()
        {
            bool find = false;
            foreach (U u in Units)
            {
                if (u.GetComponent<UState>().newOld != NewOld.Old)
                {
                    find = true;
                    break;
                }
            }
            return !find;
        }

        /// <summary>
        /// Возвращает true если все элементы "новые"
        /// </summary>
        /// <returns>true если все элементы "новые"</returns>
        public bool IsAllUnitsNew()
        {
            bool find = false;
            foreach (U u in Units)
            {
                if (u.GetComponent<UState>().newOld != NewOld.New)
                {
                    find = true;
                    break;
                }
            }
            return !find;
        }




        /// <summary>
        /// Добавляет элемент (юнит) в список элементов (юнитов)
        /// </summary>
        /// <param name="u">Элемент (Юнит)</param>
        /// <returns></returns>
        public void AddUnit(U u)
        {
            if (u.GetComponent<UState>() == null)
            {
                throw new Exception("Have no component UState in unit 'u' ");
            }
            Units.Add(u);
        }

        /// <summary>
        /// Удаляет элемент (юнит) из списка элементов (юнитов)
        /// </summary>
        /// <param name="u">Элемент (Юнит)</param>
        /// <returns></returns>
        public bool RemoveUnit(U u)
        {
            return Units.Remove(u);
        }

        /// <summary>
        /// Возвращает первый в списке элемент
        /// </summary>
        /// <returns>Возвращаемый первый элемент</returns>
        public U GetFirstUnit()
        {
            return Units.First();
        }

        public void Shuffle(Random RandomGenerator)
        {

           

            for (int x = 0; x < 1000; x++)
            {

                int i = RandomGenerator.Next(0,Units.Count);
                int j = RandomGenerator.Next(0,Units.Count);
                // обменять значения data[j] и data[i]
                var temp = Units[j];
                Units[j] = Units[i];
                Units[i] = temp;
            }
            //for (int i = Units.Count - 1; i >= 0; i--)
            //{
            //    int j = random.Next(i + 1);
            //    // обменять значения data[j] и data[i]
            //    var temp = Units[j];
            //    Units[j] = Units[i];
            //    Units[i] = temp;
            //}
        }


        #endregion

    }
}
