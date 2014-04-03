using System;
using Server;
using Server.Targeting;

namespace Server.Engines.PartySystem
{
	public class AddPartyTarget : Target
	{
		public AddPartyTarget( Mobile from ) : base( 8, false, TargetFlags.None )
		{
			from.SendMessage( "Qui voulez-vous ajouter � votre groupe?" ); // Who would you like to add to your party?
		}

		protected override void OnTarget( Mobile from, object o )
		{
			if ( o is Mobile )
			{
				Mobile m = (Mobile)o;
				Party p = Party.Get( from );
				Party mp = Party.Get( m );

				if ( from == m )
					from.SendMessage( "Vous ne pouvez vous ajouter � votre propre groupe" ); // You cannot add yourself to a party.
				else if ( p != null && p.Leader != from )
					from.SendMessage( "Vous devez �tre le meneur pour ajouter des membres � votre groupe" ); // You may only add members to the party if you are the leader.
				else if ( m.Party is Mobile )
					return;
				else if ( p != null && (p.Members.Count + p.Candidates.Count) >= Party.Capacity )
					from.SendMessage( "Un groupe ne comporte que 10 membres. Cela inclus ceux qui sont invit�s" ); // You may only have 10 in your party (this includes candidates).
				else if ( !m.Player && m.Body.IsHuman )
					m.SayTo( from, "Non, je pr�f�re rester ici et regarder des clous rouiller" ); // Nay, I would rather stay here and watch a nail rust.
				else if ( !m.Player )
					from.SendMessage( "Cette cr�ature ignore votre offre" ); // The creature ignores your offer.
				else if ( mp != null && mp == p )
					from.SendMessage( "Cette personne est d�j� dans votre groupe!" ); // This person is already in your party!
				else if ( mp != null )
					from.SendMessage( "Cette personne est d�j� dans un groupe!" ); // This person is already in a party!
				else
					Party.Invite( from, m );
			}
			else
			{
				from.SendMessage( "Vous ne pouvez ajouter que des cr�atures vivantes � votre groupe!" ); // You may only add living things to your party!
			}
		}
	}
}