using System;
using Nananami.Commands;
using Nananami.Helpers;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Nananami.Actors
{
    public struct AutoMoveActorInitializationParameter
    {
        public float x, y;
        // angle -> 移動するときに使う角度（ラジアン）
        // speed -> 速度
        // rotateOffset -> 見た目の角度オフセット(rotatableに影響されずに適用される、ラジアン)
        public float angle, speed, rotateOffset;
        public bool rotatable; // 見た目をangleに応じて回転させるか
        public int deletionResistance; // 削除への抵抗性（挙動はアクターによる。）0以下になったら削除
    }

    public class AutoMoveActor : Actor
    {
        public void AutoMoveInitialize(AutoMoveActorInitializationParameter param)
        {
            var pos = transform.localPosition;
            pos.x = param.x;
            pos.y = param.y;
            transform.localPosition = pos;
            scheduler.EnqueueCommand(new SetVariable<float>("x", param.x));
            scheduler.EnqueueCommand(new SetVariable<float>("y", param.y));
            scheduler.EnqueueCommand(new SetVariable<float>("angle", param.angle));
            scheduler.EnqueueCommand(new SetVariable<float>("speed", param.speed));
            scheduler.EnqueueCommand(new SetVariable<float>("rotateOffset", param.rotateOffset));
            scheduler.EnqueueCommand(new SetVariable<bool>("rotatable", param.rotatable));
            scheduler.EnqueueCommand(new SetVariable<int>("deletionResistance", param.deletionResistance));

            scheduler.Execute(); // 変数をセットするためにいったん実行しておく

            m_applyTransform();

            m_is_automove_initialized = true;
        }

        protected override void m_update()
        {
            base.m_update();
            if (!m_is_automove_initialized)
            {
                throw new InvalidOperationException($"This auto move actor is not initialized.");
            }
        }

        protected override void m_updateAfterCommandExecution()
        {
            m_updatePosition();
            m_applyTransform();

            scheduler.SetVariableImmediate("x", transform.localPosition.x);
            scheduler.SetVariableImmediate("y", transform.localPosition.y);
        }

        private void m_applyTransform()
        {
            var pos = transform.localPosition;
            pos.x += m_x_delta;
            pos.y += m_y_delta;
            transform.localPosition = pos;

            // 回転を適用
            bool rotatable = m_getVariable<bool>("rotatable");
            if (rotatable)
            {
                float angle = m_getVariable<float>("angle");
                float rotateOffset = m_getVariable<float>("rotateOffset");
                float totalRotation = angle + rotateOffset;
                transform.rotation = Quaternion.Euler(0, 0, totalRotation * Mathf.Rad2Deg);
            }
            else
            {
                float rotateOffset = m_getVariable<float>("rotateOffset");
                transform.rotation = Quaternion.Euler(0, 0, rotateOffset * Mathf.Rad2Deg);
            }
        }

        protected virtual void m_updatePosition()
        {
            float angle = m_getVariable<float>("angle");
            float speed = m_getVariable<float>("speed");

            m_x_delta = (float)Math.Cos(angle) * speed;
            m_y_delta = (float)Math.Sin(angle) * speed;
        }

        protected T m_getVariable<T>(string name)
        {
            T result = CommandVariableHelper.GetVariable<T>(scheduler, name);
            return result;
        }

        private float m_x_delta, m_y_delta;
        private bool m_is_automove_initialized = false;
    }
}