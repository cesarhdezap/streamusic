using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Logica.Clases;
using System.Linq;
using Java.Security;

namespace StreamusicClientAndroid
{
    class ListaDeCancionesRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ListaDeCancionesRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<ListaDeCancionesRecyclerViewAdapterClickEventArgs> ItemLongClick;
        public event EventHandler<ListaDeCancionesRecyclerViewAdapterContextMenuEventArgs> ContextMenuOpen;

        Cancion[] items;

        public ListaDeCancionesRecyclerViewAdapter(Cancion[] data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.layout_lista_listitem;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new ListaDeCancionesRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as ListaDeCancionesRecyclerViewAdapterViewHolder;
            holder.TextViewCancion.Text = item.Nombre;
            holder.TextViewArtista.Text = item.Artistas.First().Nombre;
            holder.Cancion = item;
            
        }

        public override int ItemCount => items.Length;

        void OnClick(ListaDeCancionesRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ListaDeCancionesRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class ListaDeCancionesRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder, View.IOnCreateContextMenuListener
    {
        public TextView TextViewCancion { get; set; }
        public TextView TextViewArtista { get; set; }
        public Cancion Cancion { get; set; }
        public CardView ElementoRoot { get; set; }


        public ListaDeCancionesRecyclerViewAdapterViewHolder(View itemView, Action<ListaDeCancionesRecyclerViewAdapterClickEventArgs> clickListener, Action<ListaDeCancionesRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            TextViewCancion = itemView.FindViewById<TextView>(Resource.Id.listaItemTextViewNombreCancion);
            TextViewArtista = itemView.FindViewById<TextView>(Resource.Id.listaItemTextViewNombreArtista);
            ElementoRoot = itemView.FindViewById<CardView>(Resource.Id.cardview_listitem_cancion);

            itemView.Click += (sender, e) => clickListener(new ListaDeCancionesRecyclerViewAdapterClickEventArgs { View = itemView, Cancion = Cancion , Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ListaDeCancionesRecyclerViewAdapterClickEventArgs { View = itemView, Cancion = Cancion, Position = AdapterPosition });
            ElementoRoot.SetOnCreateContextMenuListener(this);
            
            
        }

        

        public void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            menu.Add(AdapterPosition, (int)IdsContextMenu.AgregarAlFinalDeCola, 0, "Agregar final a cola");
            menu.Add(AdapterPosition, (int)IdsContextMenu.AgregarSiguienteEnCola, 1, "Agregar siguiente a cola");
            menu.Add(AdapterPosition, (int)IdsContextMenu.AgregarALista, 2, "Agregar a lista");
        }
    }

    public enum IdsContextMenu
    {
        AgregarAlFinalDeCola = 121,
        AgregarSiguienteEnCola = 122,
        AgregarALista = 123
    }

    public class ListaDeCancionesRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public Cancion Cancion { get; set; }
        public int Position { get; set; }
    }

    public class ListaDeCancionesRecyclerViewAdapterContextMenuEventArgs : EventArgs
    {
        public Cancion Cancion { get; set; }
    }
}