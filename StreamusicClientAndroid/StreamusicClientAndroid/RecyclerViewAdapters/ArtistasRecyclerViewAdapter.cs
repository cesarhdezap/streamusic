using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Logica.Clases;
using Android.Graphics;

namespace StreamusicClientAndroid
{
    class ArtistasRecyclerViewAdapter : RecyclerView.Adapter
    {
        public event EventHandler<ArtistasRecyclerViewAdapterClickEventArgs> ItemClick;
        public event EventHandler<ArtistasRecyclerViewAdapterClickEventArgs> ItemLongClick;
        Artista[] items;

        public ArtistasRecyclerViewAdapter(Artista[] data)
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

            var vh = new ArtistasRecyclerViewAdapterViewHolder(itemView, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var item = items[position];

            // Replace the contents of the view with that element
            var holder = viewHolder as ArtistasRecyclerViewAdapterViewHolder;
            holder.TextView.Text = item.Nombre;
            holder.Artista = item;
            if (item.Ilustracion != null)
            {
                holder.ImageView.SetImageBitmap(BitmapFactory.DecodeByteArray(item.Ilustracion, 0, item.Ilustracion.Length));
            }
        }

        public override int ItemCount => items.Length;

        void OnClick(ArtistasRecyclerViewAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ArtistasRecyclerViewAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);

    }

    public class ArtistasRecyclerViewAdapterViewHolder : RecyclerView.ViewHolder
    {
        public TextView TextView { get; set; }
        public ImageView ImageView { get; set; }
        public Artista Artista { get; set; }


        public ArtistasRecyclerViewAdapterViewHolder(View itemView, Action<ArtistasRecyclerViewAdapterClickEventArgs> clickListener,
                            Action<ArtistasRecyclerViewAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            TextView = itemView.FindViewById<TextView>(Resource.Id.txtNombre);
            ImageView = itemView.FindViewById<ImageView>(Resource.Id.imagen);
            
            itemView.Click += (sender, e) => clickListener(new ArtistasRecyclerViewAdapterClickEventArgs { View = itemView, Artista = Artista, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ArtistasRecyclerViewAdapterClickEventArgs { View = itemView, Artista =Artista, Position = AdapterPosition });
        }
    }

    public class ArtistasRecyclerViewAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public Artista Artista { get; set; }
        public int Position { get; set; }
    }
}