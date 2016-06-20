﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Cirrious.FluentLayouts.Touch;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MBAutoComplete
{
	[Register("MBAutoCompleteTextField")]
	public class MBAutoCompleteTextField : UITextField, IUITextFieldDelegate
	{
		public ISortingAlghorithm SortingAlghorithm
		{
			get;
			set;
		} = new DefaultSortingAlgorithm();

		public AutoCompleteDataSource DataSource
		{
			get;
			set;
		} = new DefaultDataSource();

		public UITableView AutoCompleteTableView
		{
			get;
			private set;
		}

		public IDataFetcher DateFetcher
		{
			get;
			set;
		}

		private UIViewController ViewToAddTo;

		public MBAutoCompleteTextField(IntPtr ptr) : base(ptr)
		{
			
		}

		public void Setup(UIViewController view, List<string> suggestions)
		{
			ViewToAddTo = view;
			DateFetcher  = new DefaultDataFetcher(suggestions);
			initialize();
		}

		public void Setup(UIViewController view, IDataFetcher fetcher)
		{
			ViewToAddTo = view;
			DateFetcher = fetcher;
			initialize();
		}

		private void initialize()
		{
			//Make new tableview and do some settings
			AutoCompleteTableView = new UITableView();
			AutoCompleteTableView.Layer.CornerRadius = 5; //rounded corners
			AutoCompleteTableView.ContentInset = UIEdgeInsets.Zero;
			AutoCompleteTableView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight; //for resizing (switching from table to portait for example)
			AutoCompleteTableView.Bounces = false;
			AutoCompleteTableView.BackgroundColor = UIColor.Clear;
			AutoCompleteTableView.Hidden = true;
			AutoCompleteTableView.TranslatesAutoresizingMaskIntoConstraints = false;
			AutoCompleteTableView.Source = this.DataSource;

			//Some textfield settings
			this.TranslatesAutoresizingMaskIntoConstraints = false;
			this.Delegate = this;
			this.AutocorrectionType = UITextAutocorrectionType.No;
			this.ClearButtonMode = UITextFieldViewMode.WhileEditing;

			Superview.InsertSubviewAbove(AutoCompleteTableView, ViewToAddTo.View);
		
			// Check if the superview is a uitableviewcell, by doing this we know that the parent is a uitableviewcontroller
			Type type = Superview.Superview?.GetType();

			// Disable clip to bounds to present the suggestions tableview properly
			if (type != null && type == typeof(UITableViewCell))
			{
				UITableViewCell cell = (UIKit.UITableViewCell)Superview.Superview;
				cell.ClipsToBounds = false;

				var tableViewController = ViewToAddTo as UITableViewController;
				AutoCompleteTableView.Layer.ZPosition = 1000;
				tableViewController.TableView.Layer.ZPosition = 999;
			}

			//add constraints
			Superview.AddConstraints(
				AutoCompleteTableView.AtTopOf(this, this.Frame.Height - 5),
				AutoCompleteTableView.WithSameWidth(this),
				AutoCompleteTableView.WithSameLeft(this),
				AutoCompleteTableView.Height().EqualTo(150)
			);


			DataSource.AutoCompleteTextField = this; //ugly hack?
										
			//listen to edit events
			this.AllEditingEvents += async (sender,eventargs) =>
			{
				if (this.Text.Length > 2)
				{

					await showAutoCompleteView();
				}
				else
				{
					hideAutoCompleteView();
				}
			};

		}

		private async Task showAutoCompleteView()
		{
			AutoCompleteTableView.Hidden = false;

			var parentTable = ViewToAddTo as UITableViewController;
			if (parentTable != null) //if there is a parenttable
			{
				//parentTable.TableView.UserInteractionEnabled = false;
				//AutoCompleteTableView.UserInteractionEnabled = true;
				//this.UserInteractionEnabled = true;
			}

			await UpdateTableViewData();
		}

		private void hideAutoCompleteView()
		{
			AutoCompleteTableView.Hidden = true;

			var parentTable = ViewToAddTo as UITableViewController;
			if (parentTable != null) //if there is a parenttable
			{
				//parentTable.TableView.UserInteractionEnabled = true;
			}
		}

		public async Task UpdateTableViewData(){

			await DateFetcher.PerformFetch(
				delegate (List<string> unsortedData)
			{
				var sorted = this.SortingAlghorithm.DoSort(unsortedData);

				this.DataSource.Suggestions = sorted;

				AutoCompleteTableView.ReloadData();
			}
			                              );
		}

	}
}
