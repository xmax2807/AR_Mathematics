using System.Collections;
using UnityEngine;

namespace Project.MiniGames.TutorialGames
{
    public class TransformCommand : ITutorialCommand
    {
        [System.Serializable]
        public struct TransformStruct{
            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale;

            public TransformStruct(Vector3 position, Vector3 rotation, Vector3 scale){
                this.position = position;
                this.rotation = rotation;
                this.scale = scale;
            }
            public TransformStruct(Transform transform, bool isLocal = true){
                if(isLocal){
                    this.rotation = transform.localEulerAngles;
                    this.position = transform.localPosition;
                    this.scale = transform.localScale;
                }
                else{
                    this.rotation = transform.eulerAngles;
                    this.position = transform.position;
                    this.scale = transform.localScale;
                }
            }

            public static TransformStruct operator +(TransformStruct a, TransformStruct b)
        => new TransformStruct(
            a.position + b.position,
            a.rotation + b.rotation,
            a.scale + b.scale
        );
        }
        private Transform m_source;
        private TransformStruct m_destination;
        private TransformStruct m_preverse;
        private float m_duration;

        public TransformCommand(Transform target, TransformStruct offset, float duration, bool isOffset){
            m_source = target;
            m_preverse = new TransformStruct(target);
            m_destination = isOffset? new TransformStruct(target) + offset : offset;
            m_duration = duration;
        }
        public TransformCommand CreateInvertCommand(){
            return new TransformCommand(m_source, m_preverse, m_duration, false);
        }
        public IEnumerator Execute(ICommander commander)
        {
            float timer = 0f; // Timer for the animation

            // setup positions
            Vector3 startPosition = m_source.localPosition;
            Vector3 targetPosition = m_destination.position;

            //setup scales
            Vector3 startScale = m_source.localScale;
            Vector3 targetScale = m_destination.scale;

            //setup rotations
            Vector3 startRotation = m_source.localEulerAngles;
            Vector3 targetRotation = m_destination.rotation;

            while (timer < m_duration)
            {
                // Calculate the current position based on the timer and duration
                float lerpTValue = timer / m_duration;
                Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, lerpTValue);
                Vector3 currentScale = Vector3.Lerp(startScale, targetScale, lerpTValue);
                Vector3 currentRotation = Vector3.Lerp(startRotation, targetRotation, lerpTValue);

                // Set the local position of the game object
                m_source.localPosition = currentPosition;
                m_source.localScale = currentScale;
                m_source.localEulerAngles = currentRotation;

                // Increase the timer
                timer += Time.deltaTime;

                yield return null; // Wait for the next frame
            }
        }

        // public IEnumerator Undo(ICommander commander)
        // {
        //     float timer = 0f; // Timer for the animation

        //     // setup positions
        //     Vector3 startPosition = m_source.localPosition;
        //     Vector3 targetPosition = m_preverse.position;

        //     //setup scales
        //     Vector3 startScale = m_source.localScale;
        //     Vector3 targetScale = m_preverse.scale;

        //     //setup rotations
        //     Vector3 startRotation = m_source.localEulerAngles;
        //     Vector3 targetRotation = m_destination.rotation;

        //     while (timer < m_duration)
        //     {
        //         // Calculate the current position based on the timer and duration
        //         float lerpTValue = timer / m_duration;
        //         Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, lerpTValue);
        //         Vector3 currentScale = Vector3.Lerp(startScale, targetScale, lerpTValue);
        //         Vector3 currentRotation = Vector3.Lerp(startRotation, targetRotation, lerpTValue);

        //         // Set the local position of the game object
        //         m_source.localPosition = currentPosition;
        //         m_source.localScale = currentScale;
        //         m_source.localEulerAngles = currentRotation;

        //         // Increase the timer
        //         timer += Time.deltaTime;

        //         yield return null; // Wait for the next frame
        //     }
        // }
    }
}