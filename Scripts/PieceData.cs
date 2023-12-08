using System;

namespace _Project.Scripts
{
    [Serializable]
    public class PieceData
    {
        public PieceType pieceType;
        public int pieceWorth = 1;

        public PieceData(PieceType pieceType, int pieceWorth)
        {
            this.pieceType = pieceType;
            this.pieceWorth = pieceWorth;
        }
    }
}