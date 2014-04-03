using System;
using Server;
using System.Reflection;
using Server.Items;
using System.Collections;
using Server.Commands;
using System.Collections.Generic;
using Server.Gumps;
using Server.Mobiles;
using Server.Network;

namespace Server.Commands 
{ 
   public class Find
   { 
      public static void Initialize()
      { 
         CommandSystem.Register( "Find", AccessLevel.Counselor, new CommandEventHandler( Find_OnCommand ) ); 
      } 

      [Usage( "Find <TypeName|Serial>" )]
      [Description( "Recherche d'item par le type ou par le serial" )]
      public static void Find_OnCommand( CommandEventArgs e ) 
      { 
         Mobile from = e.Mobile;
         string arg="";
         Type type=null;
         List <Item> items = new List<Item>();
         List <Mobile> mobiles = new List<Mobile>();
         
         if (e.Arguments.Length > 0)
         {
           	arg = e.Arguments[0]; 
           	
           	type = ScriptCompiler.FindTypeByName( arg );
         }
         
         if(type!=null)
         {
         	if (type.IsSubclassOf(typeof(Item)) || type.Equals(typeof(Item)))
            {
                Dictionary<Serial, Item>.Enumerator its = World.Items.GetEnumerator();
                List<Item> alItems = new List<Item>();
                while (its.MoveNext()) alItems.Add(its.Current.Value);
                its.Dispose();

                for (int k = 0; k < alItems.Count; ++k)
                {
                    Item it = alItems[k] as Item;
                                            
                    if (it.GetType().Equals(type))
                    {
                    	items.Add(it);
                    }
                }
            }
            else if (type.IsSubclassOf(typeof(Mobile)) || type.Equals(typeof(Mobile)))
            {
                Dictionary<Serial, Mobile>.Enumerator mobs = World.Mobiles.GetEnumerator();
                List<Mobile> alMobiles = new List<Mobile>();
                while (mobs.MoveNext()) alMobiles.Add(mobs.Current.Value);
                mobs.Dispose();

                for (int k = 0; k < alMobiles.Count; ++k)
                {
                    Mobile mob = alMobiles[k] as Mobile;
                    
                    if (mob.GetType().Equals(type))
                    {
                    	mobiles.Add(mob);
                    }
                }
            }
            
            if(items.Count>0)
            {
            	from.SendGump(new FindGump(from,items,0,arg,e));
            }
            else if(mobiles.Count>0)
            {
            	from.SendGump(new FindGump(from,mobiles,0,arg,e));
            }
            
         }
         else from.SendMessage(arg+" n'est pas un type valide.");
         
      }
      
      private class FindGump:Gump
      {
      	private List<Item> items= new List<Item>();
      	private List<Mobile> mobiles = new List<Mobile>();
      	private Mobile from;
      	private int index;
      	private string type;
      	private CommandEventArgs cmde;
      	
      	public FindGump(Mobile f, List<Item> its,int i,string arg,CommandEventArgs e): base( 0, 0 )
      	{
      		items = its;
      		from = f;
      		index = i;
      		type = arg;
      		cmde = e;
      		Initialize();
      	}
      	
      	public FindGump(Mobile f, List<Mobile> mobs,int i,string arg,CommandEventArgs e): base( 0, 0 )
      	{
      		mobiles = mobs;
      		from = f;
      		index =i;
      		type = arg;
      		cmde = e;
      		Initialize();
      	}
      	
      	private void Initialize()
      	{
      		Closable=true;
			Disposable=true;
			Dragable=true;
			Resizable=false;
			
			Map m_Map = from.Map;
			if(items.Count>0 )m_Map = items[index].Map;
			if(mobiles.Count>0 )m_Map = mobiles[index].Map;
			
			Point3D m_Location;
			m_Location = items.Count>0?items[index].Location:mobiles[index].Location;
			
			AddBackground(0, 0, 182, 109, 9200);
			AddButton(8, 78, 4014, 4015, 1, GumpButtonType.Reply, 0);
			AddButton(142, 78, 4005, 4006, 2, GumpButtonType.Reply, 0);
			AddLabel(8, 40, 0, m_Location.ToString());
			AddLabel(8, 10, 0, type);
			AddLabel(115, 40, 0, m_Map.Name);
			AddLabel(115, 10, 0, (index+1).ToString()+"/"+(items.Count>0?items.Count.ToString():mobiles.Count.ToString()));
			
			if(m_Map!=Map.Internal)from.Map = m_Map;
			from.Location = m_Location;
		}
      	
      	public override void OnResponse(NetState state, RelayInfo info )
      	{
      		int idx = info.ButtonID;
						
      		if(idx==1) 
      		{
      			if(index>0)index--;
      			else
      			{
      				index=items.Count>0?(items.Count-1):(mobiles.Count-1);
      			}
      		}
      		      		
      		if(idx==2)
      		{
      			index++;
      			if((items.Count>0 && index==items.Count)||(mobiles.Count>0 && index==mobiles.Count))index=0;
      		}
      		
      		if(idx==1 || idx==2)
      		{
      			if(items.Count>0)
      			{
      				for(int z=0;z<items.Count;z++)
      				{
      					if(items[z].Deleted)
      					{
      						Find.Find_OnCommand(cmde);
      						return;
      					}
      				}
      				      					
      				from.SendGump(new FindGump(from,items,index,type,cmde));
      			}
      			if(mobiles.Count>0)
      			{
      				for(int z=index;z<mobiles.Count;z++)
      				{
      					if(mobiles[z].Deleted || (type=="joueur" && mobiles[z].Map==Map.Internal))
      					{
      						Find.Find_OnCommand(cmde);
      						return;
      					}
      				}
      					
      				from.SendGump(new FindGump(from,mobiles,index,type,cmde));
      			}
      		}
      		
      	}
      }
   } 
}
