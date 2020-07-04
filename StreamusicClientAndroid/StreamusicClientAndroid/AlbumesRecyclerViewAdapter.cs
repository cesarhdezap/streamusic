using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Logica.Clases;
using Android.Graphics;

namespace StreamusicClientAndroid
{
    public class AlbumesRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<RecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<RecyclerViewAdapterClickEventArgs> ItemLongClick;
        Album[] items;

        public AlbumesRecyclerViewAdapter(Album[] data)
        {
            items = data;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            //Setup your layout here
            View itemView = null;
            var id = Resource.Layout.layout_listitem;
            itemView = LayoutInflater.From(parent.Context).
                   Inflate(id, parent, false);

            var vh = new RecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];
            

            // Replace the contents of the view with that element
            var holder = viewHolder as RecyclerViewAdapterViewHolder;
            holder.TextView.Text = item.Nombre;
            holder.Album = item;
            if(item.Ilustracion != null)
            {
                holder.ImageView.SetImageBitmap(BitmapFactory.DecodeByteArray(item.Ilustracion, 0, item.Ilustracion.Length));
            }
        }

        public override int ItemCount => items.Length;

        void OnClick(RecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(RecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class RecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextView { get; set; }
        public ImageView ImageView { get; set; }
        public Album Album { get; set; }


        public RecyclerViewAdapterViewHolder(View itemView, Action<RecyclerViewAdapterClickEventArgs> clickListener, Action<RecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            TextView = itemView.FindViewById<TextView>(Resource.Id.txtNombre);
            ImageView = itemView.FindViewById<ImageView>(Resource.Id.imagen);

            itemView.Click += (sender, e) => clickListener(new RecyclerViewAdapterClickEventArgs { View = itemView, Album = Album, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new RecyclerViewAdapterClickEventArgs { View = itemView, Album = Album, Position = AdapterPosition });
        }
    }

    public class RecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public Album Album { get; set; }
        public int Position { get; set; }
    }
}