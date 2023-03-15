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
/// Contains a pplayer object, inherits from Gameobject. Has a name field.
/// </summary>
public class Player : GameObject
{
    /// <summary>
    /// Empty if not specified.
    /// </summary>
    private string _name = string.Empty;

    /// <summary>
    /// If set, then that name - otherwise returns an empty string.
    /// </summary>
    public string Name
    {
        get => _name; 
        set => _name = value;
    }
}