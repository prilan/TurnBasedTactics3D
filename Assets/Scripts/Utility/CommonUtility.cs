using Entitas.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility
{
    public static class CommonUtility
    {
        private static int cellLayerMask = (1 << CommonConsts.CELL_LAYER_ID);

        public static Int2 CalculateCellPosition(Vector3 transformPosition)
        {
            float xPosition = (transformPosition.x + CommonConsts.FieldWidth - 1) / 2;
            float yPosition = (transformPosition.z + CommonConsts.FieldWidth - 1) / 2;

            return new Int2((int)xPosition, (int)yPosition);
        }

        public static Vector3 CalculateTransformPositionByCellposition(Int2 cellPosition)
        {
            float xPosition = cellPosition.x * 2 - CommonConsts.FieldWidth + 1;
            float zPosition = cellPosition.y * 2 - CommonConsts.FieldWidth + 1;

            return new Vector3(xPosition, 0, zPosition);
        }

        public static bool RaycastWorldPositionToCell(Vector3 position, out GameEntity cellEntity)
        {
            cellEntity = null;

            if (Physics.Raycast(position, Vector3.down, out RaycastHit hitCell, Mathf.Infinity, cellLayerMask)) {
                return PrepareCellEntity(hitCell, ref cellEntity);
            }

            return false;
        }

        public static bool RaycastScreenPointToCell(Vector3 screenPoint, out GameEntity cellEntity)
        {
            cellEntity = null;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out RaycastHit hitCell, Mathf.Infinity, cellLayerMask)) {
                return PrepareCellEntity(hitCell, ref cellEntity);
            }

            return false;
        }

        private static bool PrepareCellEntity(RaycastHit hitCell, ref GameEntity cellEntity)
        {
            GameObject cellTargetGo = hitCell.collider.gameObject;
            EntityLink cellEntityLink = cellTargetGo.GetComponentInParent<EntityLink>();

            if (cellEntityLink == null) {
                return false;
            }

            cellEntity = (GameEntity)cellEntityLink.entity;

            return true;
        }

        public static ActiveCharacterPlayer ConvertToCharacterPlayerFromEditInstrument(EditActiveInstrument editActiveInstrument)
        {
            switch (editActiveInstrument) {
                case EditActiveInstrument.CharacterA:
                    return ActiveCharacterPlayer.CharacterA;
                case EditActiveInstrument.CharacterB:
                    return ActiveCharacterPlayer.CharacterB;
                default:
                    return ActiveCharacterPlayer.CharacterA;
            }
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static List<Vector2> Clone(this List<Vector2> listToClone)
        {
            List<Vector2> resultList = new List<Vector2>();
            foreach (Vector2 item in listToClone) {
                Vector2 val = new Vector2((int)item.x, (int)item.y);
                resultList.Add(val);
            }

            return resultList;
        }
    }
}
