namespace WebServer.Services;

/// <summary> 
/// Author:    Tyler DeBruin
/// Partner:   Rayyan Hamid
/// Date:      4-27-2022
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Tyler DeBruin and Rayyan Hamid - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Tyler DeBruin and Rayyan Hamid, certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents 
///
/// Player domain object - Matches the database object. 
/// </summary>
public class PlayerSize
{
    public int PlayerSizeId { get; set; }
    public int GameId { get; set; }
    public double Size { get; set; }
    public DateTime InsertDate { get; set; }
}