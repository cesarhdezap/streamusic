using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Logica.Clases;

namespace StreamusicClientAndroid.RecyclerViewAdapters
{
    class ListasDeReproduccionRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ListasDeReproduccionRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<ListasDeReproduccionRecyclerViewAdapterClickEventArgs> ItemLongClick;
        ListaDeReproduccion[] items;

        public ListasDeReproduccionRecyclerViewAdapter(ListaDeReproduccion[] data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.layout_listareproduccion_listitem;
            itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

            var vh = new ListasDeReproduccionRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as ListasDeReproduccionRecyclerViewAdapterViewHolder;
            holder.TextViewNombreLista.Text = item.Nombre;
            holder.ListaDeReproduccion = item;
        }

        public override int ItemCount => items.Length;

        void OnClick(ListasDeReproduccionRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ListasDeReproduccionRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class ListasDeReproduccionRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextViewNombreLista { get; set; }
        public ListaDeReproduccion ListaDeReproduccion { get; set; }


        public ListasDeReproduccionRecyclerViewAdapterViewHolder(View itemView, Action<ListasDeReproduccionRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<ListasDeReproduccionRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            TextViewNombreLista = itemView.FindViewById<TextView>(Resource.Id.textviewNombreListaReproduccion);
            
            itemView.Click += (sender, e) => clickListener(new ListasDeReproduccionRecyclerViewAdapterClickEventArgs { View = itemView, ListaDeReproduccion = ListaDeReproduccion,Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ListasDeReproduccionRecyclerViewAdapterClickEventArgs { View = itemView, ListaDeReproduccion = ListaDeReproduccion, Position = AdapterPosition });
        }
    }

    public class ListasDeReproduccionRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public ListaDeReproduccion ListaDeReproduccion { get; set; }
        public int Position { get; set; }
    }
}