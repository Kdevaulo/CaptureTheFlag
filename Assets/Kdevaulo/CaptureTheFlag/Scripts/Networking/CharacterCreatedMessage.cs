using Mirror;

using UnityEngine;

namespace Kdevaulo.CaptureTheFlag.Networking
{
    public struct CharacterCreatedMessage : NetworkMessage
    {
        public int Id;
        public string PlayerName;
        public Color CharacterColor;
    }
}