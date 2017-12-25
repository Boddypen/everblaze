﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Everblaze.Miscellaneous;


namespace Everblaze.Resources
{
	public class ModelResources
	{

		public static Model
			pointerModel;

		public static Model
			treeModel;


		/// 
		/// <summary>
		///		Loads all models.
		/// </summary>
		/// 
		/// <param name="content">The game's content manager.</param>
		/// 
		public static void loadContent(ContentManager content)
		{

			// Load models.
			pointerModel = content.Load<Model>("fbx/debug/pointer");

			treeModel = content.Load<Model>("fbx/environment/tree");
			
		}


		/// 
		/// <summary>
		///		Renders a model into the 3D game world.
		/// </summary>
		/// 
		/// <param name="model">The model to be rendered.</param>
		/// <param name="camera">The camera.</param>
		/// <param name="position">The position of the model.</param>
		/// <param name="rotation">The rotation of the model.</param>
		/// <param name="scale">The scale of the model.</param>
		/// 
		public static void renderModel(
			Model model,
			Camera camera,
			Vector3 position,
			Vector3 rotation,
			Vector3 scale)
		{

			Matrix view = Matrix.CreateLookAt(camera.position, camera.target, Vector3.Up);

			Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90.0F), 1920.0F / 1080.0F, 0.1F, 100.0F);
			
			Matrix world = 
				Matrix.CreateScale(scale) *
				Matrix.CreateRotationX(rotation.X) *
				Matrix.CreateRotationY(rotation.Y) *
				Matrix.CreateRotationZ(rotation.Z) *
				Matrix.CreateTranslation(position);
			
			foreach(ModelMesh mesh in model.Meshes)
			{
				foreach(BasicEffect effect in mesh.Effects)
				{
					effect.World = world;
					effect.View = view;
					effect.Projection = projection;



					effect.LightingEnabled = true;
					effect.PreferPerPixelLighting = false;
					effect.DirectionalLight0.Enabled = true;
					effect.DirectionalLight0.Direction = new Vector3(-0.5F, -5.0F, -1.5F);
					effect.DirectionalLight0.DiffuseColor = new Vector3(0.25F);
					effect.DirectionalLight0.SpecularColor = new Vector3(0.01F, 0.01F, 0.01F);
					effect.AmbientLightColor = new Vector3(0.75F, 0.75F, 0.75F);
				}

				mesh.Draw();
			}

		}

	}
}
