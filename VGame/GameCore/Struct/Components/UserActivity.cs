using System;
using System.Collections.Generic;
using System.Windows.Input;
using VGameCore.Abstract;

namespace VGameCore.Struct.Components
{
     
    /// <summary>
    /// Инкапсулирует нечто вроде команды. 
    /// Пользователю нужно создать команду закинув туда:
    /// 1. Условие реакции (если пользовательский ввод такой - то выполняем/не выполняем команду)
    /// 2. Саму рекацию на событие пользовательского ввода
    /// 3. Параметры для реакции (чтобы она была правильной)
    /// </summary>
    public class UserActivityCommand
    {

        public BoolUserDoSomething ConditionExecution { get; }
        private FullFreeDelegate Execute;
        public List<object> Parameters;
        
        /// <summary>
        /// Создает "команду"
        /// </summary>
        /// <param name="conditionExecution">Условие выполнения команды (реакции)</param>
        /// <param name="execute">Сама команда - собственно реакция</param>
        /// <param name="parameters">Параметры, которые могут быть переданы в реакцию</param>
        public UserActivityCommand(BoolUserDoSomething conditionExecution, FullFreeDelegate execute, List<object> parameters)
        {
            ConditionExecution = conditionExecution;
            Execute = execute;
            Parameters = parameters;
        }

        /// <summary>
        /// Само выполнение команды
        /// </summary>
        public void go()
        {
            Execute(Parameters.ToArray());
        }

    }
    /// <summary>
    /// Класс тесно связан с TUserActivity (подписывается на действия пользователя через статический класс Game). Описывает нечто вроде паттерна команда. 
    /// 1. Подписывается на все вообще события пользовательского ввода, которые прокидываются через событие  TUserActivity.UserDoSomethingEvent.
    /// 2. Пользователь заполняет список команд (см. класс UserActivityCommand)
    /// 3. При событии UserDoSomethingEvent весь список проверяется - если условия выполнения команды правильные - команда выполняется 
    /// </summary>
    public class UserActivity : Component
    {
        #region constructors
        public UserActivity(string name, IComponentContainer container) : base(name, container)
        {
            Game.UserActivity.UserDoSomethingEvent += UserDoSomethingEvent;
        }


        #endregion

        #region variables
        #endregion

        #region properties
        /// <summary>
        /// Набор реакций на определенную активность пользователя. Объект UserActivityCommand содержит условие выполнения и саму реакцию.
        /// </summary>
        public List<UserActivityCommand> Reactions { get; set; }
        #endregion

        #region methods
        /// <summary>
        /// Проверка списка реакций (команд) на некое действие пользователя.
        /// </summary>
        /// <param name="mouse">Состояние мыши</param>
        /// <param name="mousebutton">Нажатая клавиша мыши</param>
        /// <param name="key">Нажатая клавиша клавиатуры</param>
        private void UserDoSomethingEvent(MouseEventArgs mouse, MouseButtonEventArgs mousebutton, KeyEventArgs key)
        {
            foreach (UserActivityCommand R in Reactions)
                if (R.ConditionExecution(mouse, mousebutton, key))
                    R.go();
        }
        #endregion

    }
}
