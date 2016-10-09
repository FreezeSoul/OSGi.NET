using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OsgiDllTool.Model
{
    /// <summary>
    /// 实体基类，所有数据实体都是从该类继承的
    /// </summary>
   
    public class EntityBase : INotifyPropertyChanged
    {
        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion


        #region Public Methods and Operators

        public virtual bool Check(out string message)
        {
            var type = this.GetType();
            message = null;
            foreach (var propertyInfo in type.GetProperties())
            {
                message = CheckValueAttribute.Check(this, propertyInfo);
                if (!string.IsNullOrEmpty(message))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        protected virtual void OnPropertyChanged(Expression<Func<object>> expression)
        {
            if (expression != null)
            {
                if (expression.NodeType == ExpressionType.Lambda)
                {
                    MemberExpression body = null;
                    if (expression.Body is UnaryExpression)
                    {
                        var ue = expression.Body as UnaryExpression;
                        body = ue.Operand as MemberExpression;
                    }
                    else
                    {
                        body = expression.Body as MemberExpression;
                    }

                    if (body != null)
                    {
                        string propertyName = body.Member.Name;
                        OnPropertyChanged(propertyName);
                    }
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// The check value attribute.
    /// </summary>
    public class CheckValueAttribute : Attribute
    {
        #region Constructors and Destructors

        public CheckValueAttribute(string message)
        {
            this.Message = message;
        }

        #endregion

        #region Public Properties

        public string Message { get; set; }

        public string Regular { get; set; }

        #endregion

        #region Public Methods and Operators

        public static string Check(object obj, PropertyInfo info)
        {
            var value = info.GetValue(obj, null);
            var attrs = info.GetCustomAttributes(typeof(CheckValueAttribute), false);
            if (attrs.Length == 0)
            {
                return null;
            }

            var attr = (CheckValueAttribute)attrs[0];
            if (value == null)
            {
                return attr.Message;
            }

            // 没有正则表达式时判断是否为空
            if (string.IsNullOrEmpty(attr.Regular))
            {
                return string.IsNullOrEmpty(value.ToString()) ? attr.Message : null;
            }

            var regular = new Regex(attr.Regular, RegexOptions.IgnoreCase);

            // 正则表达式判断条件是否成立
            if (regular.IsMatch(value.ToString()))
            {
                return null;
            }

            return attr.Message;
        }

        #endregion
    }
}