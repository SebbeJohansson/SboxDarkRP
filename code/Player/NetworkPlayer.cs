using System;
using GameSystems.Jobs;
using GameSystems.Player;
using Sandbox.GameSystems.Database;
using Sandbox.GameSystems.Player;
using Sandbox.UI;

namespace Sandbox.GameSystems.Player
{
	/// <summary>
	/// Represents a DarkRP player 
	/// </summary>
	public struct NetworkPlayer
	{
		public List<UserGroup> UserGroups { get; set; }
		public GameObject GameObject { get; set; }
		public Connection Connection { get; set; }
		public string Name { get; set; }
		public JobResource Job { get; set; }

		public NetworkPlayer(GameObject gameObject, Connection connection, List<UserGroup> userGroups)
		{
			GameObject = gameObject;
			Connection = connection;
			Name = connection.DisplayName;
			UserGroups = userGroups;
			Job = JobProvider.GetDefault();

			Log.Info("Loading saved data if exists");
			if (FileSystem.Data.FileExists("playersdata/" + Connection.SteamId))
			{
				Log.Info("Loading savedPlayer");
				var savedPlayer = SavedPlayer.LoadSavedPlayer(Connection.SteamId);

				Log.Info("SavedPlayer SteamID: " + savedPlayer.SteamId + " Money: " + savedPlayer.Money);

				// Overwriting default data with saved Data
				GameObject.Components.Get<Player>().SetBalance(savedPlayer.Money);
				Log.Info("LoadedPlayer SteamID: " + Connection.SteamId + " Money: " + GameObject.Components.Get<Player>().Balance);
			}
			Log.Info("Ended player creation");
		}

		/// <summary>
		/// Checks if the player is part of a UserGroup with the required permission level.
		/// Returns true if the player has the required permission level.
		/// </summary>
		public bool CheckPermission(PermissionLevel permissionLevel)
		{
			foreach (var userGroup in UserGroups)
			{
				if (userGroup.PermissionLevel >= permissionLevel)
				{
					return true;
				}
			}
			return false;
		}

		public void SetRank(UserGroup userGroup)
		{
			UserGroups.Clear();
			UserGroups.Add(userGroup);
		}

		public void AddRank(UserGroup userGroup)
		{
			UserGroups.Add(userGroup);
		}
		
		// Add the GetHighestUserGroup method
		public UserGroup GetHighestUserGroup()
		{
			return UserGroups.MaxBy(x => x.PermissionLevel);
		}
	}
}
