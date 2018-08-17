# .net flux c# library for WPF/Windows apps
A tiny but powerful library to build WPF apps implementing FLUX(unidirectional data flow) architecture, inspired by Facebook’s flux architecture and an awesome front-end language named Elm.

# About
This library comes in handy if you are looking to implement unidirectional data flow in your WPF or Windows app in C#. This also helps to establish loosely coupled communication between the components of your app, which is an essential feature if you are developing a data-centric app and requires several components to communicate each other to perform a required task.
	
![GitHub Logo](https://github.com/yadalis/dotnet-flux-csharp-lib/blob/master/Images/connect_app_pic.png)

	
# Flow
	1. A View (WPF XAML) is designed to contain a Store along with its view layout
	
	2. All Stores contains its own Model and implements Register method to sign up for receiving the 
      messages/actions from any View(s)/Store(s)
	
	3. View dispatches a message/action with some data upon any action (button click)
	
	4. By design, all Stores receive that message/action and pull the data sent across in the message,
      does some business logic, access some external data, updates Model and sends a private message
      to its own View
	
	5. The View receives the message sent from its Store and take some actions to re-render
      the view based on the updates done to Model

# Sample Actions:
    public static class PMActions
    {
        public static void DispatchRemovePMItemUcMessage(PMItem PMItemUc)
        {
            Dispatcher.Dispatch(new BusinessAction()
              { Name = nameof(PMActions.DispatchRemovePMItemUcMessage), Data = PMItemUc });
        }

        public static void DispatchAddPMItemUcdMessage(PMItem PMItemUc)
        {
            Dispatcher.Dispatch(new BusinessAction() 
              { Name = nameof(DispatchAddPMItemUcdMessage), Data = PMItemUc });
        }

        public static void DispatchPMJobStepItemDeleteRequestedMessage(PMItem PMItemUc)
        {
            Dispatcher.Dispatch(new BusinessAction()
              { Name = nameof(DispatchPMJobStepItemDeleteRequestedMessage), Data = PMItemUc });
        }

        public static void DispatchRefreshPMViewMessage()
        {
            Dispatcher.Dispatch(new BusinessAction()
            { Name = nameof(DispatchRefreshPMViewMessage), Data = null });
        }
    }

# Sample View:
    public partial class PMItemsView : UserControl
    {
        private PMItemViewStore _pmItemViewStore { get; }

        public PMItemsView(PMItemViewStore PMItemViewStore)
        {
            _pmItemViewStore = PMItemViewStore;

            InitializeComponent();

            _pmItemViewStore.SetChangeListener(MessageName =>
            {
                if (MessageName == nameof(SWAppActions.DispatchRenderViewMessage))
                {
                    DataContext = _pmItemViewStore.ViewModel;
                }
                if (MessageName == nameof(PMActions.DispatchRefreshPMViewMessage))
                {
                    UpdateAddAllButtonBehaviour();
                    ChangeNavItemTheme();
                }
            });
        }
	}

# Sample Store:
    public class PMItemViewStore : BaseStore
    {
        public PMItemViewModel ViewModel { get; set; } = new PMItemViewModel();

        public PMItemViewStore(IPMItemRepository PMItemRepository)
        {
            Dispatcher.Register(base.ToString(), businessAction =>
            {
                switch (businessAction.Name)
                {
                    case nameof(SWAppActions.DispatchRenderViewMessage):
                        {
                            ViewModel = ((SWAppViewModel)businessAction.Data).PMItemViewModel;
                            EmitChange(businessAction.Name);
                        }
                        break;
                    case nameof(PMActions.DispatchRefreshPMViewMessage):
                        {
                            EmitChange(businessAction.Name);
                        }
                        break;
                    case nameof(PMActions.DispatchRemovePMItemUcMessage):
                        {
                            ViewModel.ItemCollection.Remove(((PMItem)businessAction.Data).DataSourceObject);
                            PMActions.DispatchRefreshPMViewMessage();
                        }
                        break;
                    case nameof(PMActions.DispatchAddPMItemUcdMessage):
                        {
                            ViewModel.ItemCollection.Add(((PMItem)businessAction.Data).DataSourceObject);
                            PMActions.DispatchRefreshPMViewMessage();
                        }
                        break;
                    default:
                        break;
                }
            });
        }
    }
    
    
# Sample View-Model
    public class PMItemViewModel : BaseViewModel
    {
        private ObservableCollection<PreventiveMaintainanceItem> _itemCollection { get; set; }

        public ObservableCollection<PreventiveMaintainanceItem> ItemCollection
        {
            get
            {
                return _itemCollection;
            }
            set
            {
                _itemCollection = value;
                NotifyPropertyChanged();
            }

        }

        public string PMMAINTViewHeaderImage => ImagePathConverter.GetImage("PMMAINTViewHeaderImage");
    }

