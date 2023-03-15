using System.Numerics;

namespace AgarioModels;

/// <summary> 
/// Author:    Tyler DeBruin and Rayyan Hamid
/// Partner:   None
/// Date:      4-9-2022
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Tyler DeBruin and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Tyler DeBruin and Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
///
/// Contains a core object - Contains information key to a unit in the world.
/// </summary>
public class GameObject
{
    /// <summary>
    /// The X Coordinate in the Game World.
    /// </summary>
    public float X
    {
        set => Location.X = value;
        get => Location.X;
    }

    /// <summary>
    /// The Y Coordinate in the Game World.
    /// </summary>
    public float Y
    {
        set => Location.Y = value;
        get => Location.Y;
    }

    /// <summary>
    /// This Object's ID, assigned by the server.
    /// </summary>
    public long ID { get; set; }

    /// <summary>
    /// Helper to Easily access the x,y coordinates in the gameworld for this object.
    /// </summary>
    public Vector2 Location;

    /// <summary>
    /// An Int that indicates the color of the object in the gameworld.
    /// </summary>
    public int ARGBColor { get; set; }

    /// <summary>
    /// The Mass of the Object.
    /// </summary>
    public float Mass { get; set; }

    /// <summary>
    /// The Radius of this object, calculated from its mass.
    /// </summary>
    /// <returns></returns>
    public double GetRadius()
    {
        return Math.Sqrt(Mass / Math.PI);
    }
    /// <summary>
    /// The Diameter of this object, calculated from its mass.
    /// </summary>
    /// <returns></returns>
    public double GetDiameter()
    {
        return Math.Sqrt(Mass / Math.PI) * 2;
    }
}