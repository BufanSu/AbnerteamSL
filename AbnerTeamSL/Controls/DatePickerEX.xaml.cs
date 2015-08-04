using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace AbnerTeamSL.Controls
{
    public static class ControlsHelper
	{
		/// <summary>
		/// 扩展DependencyObject Descendents方法，递归找到制定的控件
		/// </summary>
		/// <param name="root"></param>
		/// <returns></returns>
		public static IEnumerable<DependencyObject> Descendents(this DependencyObject root)
		{
			int count = VisualTreeHelper.GetChildrenCount(root);
			for (int i = 0; i < count; i++)
			{
				var child = VisualTreeHelper.GetChild(root, i);
				yield return child;
				foreach (var descendent in Descendents(child))
					yield return descendent;
			}
		}
	}
	public partial class DatePickerEX : System.Windows.Controls.DatePicker
	{
		public DatePickerEX()
		{
			InitializeComponent();
		}


		#region 私有字段
		DatePickerTextBox CurrentTextBox;//DatePicker的文本框控件
		Popup CurrentPopup;//DatePicker控件的TempPart
		Calendar CurrentCalendar;//日历控件
		Button CurrentButon;//DatePicker的Button控件
		string DateFormat;//定义时间显示格式
		string _InputText;

		public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DatePickerEX), new PropertyMetadata(false));
		//public static readonly DependencyProperty InputTextProperty = DependencyProperty.Register("InputText", typeof(String), typeof(DatePickerEx), new PropertyMetadata("", InputTextChanged));
		public static readonly DependencyProperty DateModeProperty = DependencyProperty.Register("DateMode", typeof(CalendarMode), typeof(DatePickerEX), new PropertyMetadata(CalendarMode.Month, DateModeChanged));
		#endregion

		#region 属性
		/// <summary>
		/// 得到手动输入的值
		/// </summary>
		public string InputText
		{
			//get { return (string)GetValue(InputTextProperty); }
			//set { SetValue(InputTextProperty, value); }
			get { return _InputText == null ? "" : _InputText; }
			private set { _InputText = value; }
		}
		/// <summary>
		/// 启用和关闭手动输入
		/// </summary>
		public bool IsReadOnly
		{
			get { return (bool)GetValue(IsReadOnlyProperty); }
			set { SetValue(IsReadOnlyProperty, value); }
		}
		///// <summary>
		/// 输入时间类型
		/// </summary>
		public CalendarMode DateMode
		{
			get { return (CalendarMode)GetValue(DateModeProperty); }
			set { SetValue(DateModeProperty, value); }
		}
		#endregion

		#region 重写方法及事件
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.CurrentTextBox = GetTemplateChild("TextBox") as DatePickerTextBox;
			this.CurrentPopup = GetTemplateChild("Popup") as Popup;
			this.CurrentButon = GetTemplateChild("Button") as Button;

			#region 注册及绑定事件
			if (null != this.CurrentTextBox)
			{
				//this.CurrentTextBox.BorderThickness = new Thickness(1);
				//this.CurrentButon.Margin = new Thickness(-25, 0, 0, 0);
				this.SetTextMode(this.CurrentTextBox);
				this.CurrentTextBox.IsReadOnly = this.IsReadOnly;
				this.CurrentTextBox.TextChanged += ((sender, e) =>
				{
					if (this.IsReadOnly)
						this.CurrentTextBox.Text = this.SelectedDate.HasValue ? this.SelectedDate.Value.ToString(this.DateFormat) : "";
					else
					{
						if (this.CurrentTextBox.Text.Trim().Length >= this.DateFormat.Length)
						{
							DateTime timeTemp;
							if (DateTime.TryParse(this.CurrentTextBox.Text.Trim(), out timeTemp))
								this.CurrentTextBox.Text = timeTemp.ToString(this.DateFormat);
						}
						this.InputText = this.CurrentTextBox.Text;
					}
					SetTextMode(this.CurrentTextBox);
				});

			}

			this.CalendarClosed += new RoutedEventHandler(DatePickerEx_CalendarClosed);


			if (this.DateMode == CalendarMode.Month)
				return;

			if (null != this.CurrentPopup)
				//利用扩展方法找到Calendar控件
				this.CurrentCalendar = this.CurrentPopup.Child.Descendents().OfType<Calendar>().FirstOrDefault();


			if (null != this.CurrentButon)
				this.CurrentButon.Click += ((sender, e) =>
				{
					this.CurrentPopup.IsOpen = true;
				});


			if (null != this.CurrentCalendar)
			{

				this.CurrentCalendar.IsTodayHighlighted = true;
				this.CurrentCalendar.DisplayModeChanged += new
						EventHandler<CalendarModeChangedEventArgs>(CurrentCalendar_DisplayModeChanged);
				this.CurrentCalendar.DisplayMode = this.DateMode;
				this.CurrentCalendar.LostMouseCapture += ((sender, e) =>
				{
					this.SelectedDate = this.DisplayDate;
				});

			}
			#endregion
		}

		void CurrentCalendar_DisplayModeChanged(object sender, CalendarModeChangedEventArgs e)
		{
			this.CurrentCalendar.DisplayModeChanged -= CurrentCalendar_DisplayModeChanged;
			this.CurrentCalendar.DisplayModeChanged += CurrentCalendar_DisplayModeChanged;
			Calendar cal = (Calendar)sender;

			//首次加载以及重新赋值DisplayMode  Calendar视图情况判断
			if (e.OldMode.Equals(CalendarMode.Year) && !e.NewMode.Equals(CalendarMode.Month))
				cal.DisplayMode = e.NewMode;
			else
				cal.DisplayMode = this.DateMode;


			//仅选择月 Calendar关闭情况判断
			if (e.NewMode.Equals(CalendarMode.Month))
				this.CurrentPopup.IsOpen = false;


			//只选择年。 Calendar 关闭情况判断
			if (this.DateMode == CalendarMode.Decade && e.NewMode == CalendarMode.Year && e.OldMode == this.DateMode)
				this.CurrentPopup.IsOpen = false;
		}
		/// <summary>
		/// 设置日期显示格式
		/// </summary>
		/// <param name="tbx"></param>
		protected void SetTextMode(DatePickerTextBox tbx)
		{
			if (null == tbx)
				return;
			switch (this.DateMode)
			{
				case CalendarMode.Year:
					DateFormat = "yyyy-MM";
					tbx.Watermark = "<yyyy-MM>";
					break;
				case CalendarMode.Decade:
					DateFormat = "yyyy";
					tbx.Watermark = "<yyyy>";
					break;
				default:
					DateFormat = "yyyy-MM-dd";
					break;
			}
			tbx.UpdateLayout();
		}
		#endregion

		//protected override 
		#region 自定义事件
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (!this.IsReadOnly)
				this.InputText = this.CurrentTextBox.Text;
		}
		protected virtual void DatePickerEx_CalendarClosed(object sender, RoutedEventArgs e)
		{
			if (null != this.SelectedDate)
				this.Text = this.SelectedDate.Value.ToString(this.DateFormat);
			this.InputText = this.Text;
		}
		//protected static void InputTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		//{
		//    DatePickerEx sender = d as DatePickerEx;
		//    if (null != sender.CurrentTextBox)
		//        sender.CurrentTextBox.Text = e.NewValue.ToString();
		//}
		protected static void DateModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			DatePickerEX sender = d as DatePickerEX;
			if (null != sender.CurrentCalendar)
				sender.CurrentCalendar.DisplayMode = (CalendarMode)e.NewValue;
			sender.SetTextMode(sender.CurrentTextBox);
		}
		#endregion

	}
}
