﻿/*
 * Created by SharpDevelop.
 * User: Dunbaratu
 * Date: 2/5/2015
 * Time: 12:47 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace kOS.UserIO
{
    /// <summary>
    /// A list of extra unicode command characters we use to genericize command codes, abstracting away
    /// the differences between different terminal models.
    /// <br/>
    /// These are all stored in the Unicode "private use" area in range [0xE000..0xF8FF].
    /// The Unicode Consortium promises that they will never make any official meanings for
    /// these characters, so they are free to use for whatever local private meanings you like.
    /// The only "public" standards for this character range are unofficial ones for
    /// very specialized interest groups.   (For example, by using this range, we have
    /// precluded people from being able to write kOS scripts in Klingon.)
    /// <br/>
    /// WARNING:  EVERY TIME you use these you must always cast them to (char).  This is because
    /// C# does not allow us to do the correct thing here, which is this:
    ///     public enum UnicodeCommand : char { stuff, stuff, stuff,....}
    /// For some reason I cannot fathom, C# lets you pick the type of any enum EXCEPT char, even
    /// though a unicode char is still a well defined narrow known number of bits and should
    /// effectively work just as well for enums as, say, a ushort.
    /// <br/>
    /// If anyone does have the ambition to get rid of the need for all the casting, they can
    /// go through here and turn all these into const chars if they like.  But that means having
    /// to manually type in their number value for each one instead of just letting the enum
    /// syntax auto-increment it for each sucessive one.
    /// </summary>
    public enum UnicodeCommand
    {
        /// <summary>
        /// Indicates an emergency low-level break signal.  Often a telnet client will send this
        /// character out-of-band immediately, queue-barging in front of whatever else is in the
        /// input queue:
        /// </summary>
        BREAK = 0xE000,
        
        /// <summary>
        /// Clear the screen and move the cursor to the upper left corner:
        /// </summary>
        CLEARSCREEN,
        
        /// <summary>
        /// Tell the terminal to resize itself to a new row/col size.  Not all terminals will be capable of doing this.
        /// This can be communicated in either direction - for the client telling the server it has been resized, or
        /// for the server telling the client to it needs to resize itself.
        /// Expects a sequence of 3 characters as follows: <br/>
        ///     RESIZESCREEN Binary_Width_Num Binary_Height_Num <br/>
        /// Where Width_num and Height_num are the numbers directly transcoded into unicode chars in a binary way.
        /// (For example a height of 66, which is hex 0x32 would end up being sent as the capital letter 'B' which is unicode 0x0032.).
        /// </summary>
        RESIZESCREEN,
        
        /// <summary>
        /// This character indicates that a string is to follow which is meant to be used to tell the terminal
        /// what it should set its titlebar to.  Output-only.
        ///     Example:  To set the terminal's title to "Vessel A, CPU 1":
        ///         TITLEBEGIN V e s s e l   A ,   C P U   1 TITLEEND
        /// </summary>
        TITLEBEGIN,

        /// <summary>
        /// This character indicates that the string that tells the terminal its title has finished.  Output-only.
        /// </summary>
        TITLEEND,

        /// <summary>
        /// Begins a cursor move to an exact position.  Expects exactly 2 more
        /// unicode chars to follow, interpreted as binary number data (not as characters)
        /// for the row num and column num, respectively.
        /// </summary>
        TELEPORTCURSOR,

        /// <summary>
        /// Indicates moving a cursor up one row.  Can be used both on output to
        /// move the cursor, or on input to encode that the arrow key was pressed.
        /// </summary>
        UPCURSORONE,
        /// <summary>
        /// Indicates moving a cursor [count] rows up.  Expects exactly 1 more unicode
        /// char to follow, interpreted as binary number data (not as character) for
        /// the number of spaces to move.
        /// </summary>
        UPCURSORNUM,
        
        /// <summary>
        /// Indicates moving a cursor down one row.  Can be used both on output to
        /// move the cursor, or on input to encode that the arrow key was pressed.
        /// </summary>
        DOWNCURSORONE, 
        /// <summary>
        /// Indicates moving a cursor [count] rows down.  Expects exactly 1 more unicode
        /// char to follow, interpreted as binary number data (not as character) for
        /// the number of spaces to move.
        /// </summary>
        DOWNCURSORNUM,

        /// <summary>
        /// Indicates moving a cursor left one column.  Can be used both on output to
        /// move the cursor, or on input to encode that the arrow key was pressed.
        /// </summary>
        LEFTCURSORONE, 
        /// <summary>
        /// Indicates moving a cursor [count] spaces left.  Expects exactly 1 more unicode
        /// char to follow, interpreted as binary number data (not as character) for
        /// the number of spaces to move.
        /// </summary>
        LEFTCURSORNUM,

        /// <summary>
        /// Indicates moving a cursor right one column.  Can be used both on output to
        /// move the cursor, or on input to encode that the arrow key was pressed.
        /// </summary>
        RIGHTCURSORONE, 
        /// <summary>
        /// Indicates moving a cursor [count] spaces right.  Expects exactly 1 more unicode
        /// char to follow, interpreted as binary number data (not as character) for
        /// the number of spaces to move.
        /// </summary>
        RIGHTCURSORNUM,
        
        /// <summary>
        /// Indicates moving a cursor to the home position of the row.  Also can be seen
        /// on input to indicate the home key being pressed.
        /// </summary>
        HOMECURSOR,

        /// <summary>
        /// Indicates moving a cursor to the end position of the row.  Also can be seen
        /// on input to indicate the end key being pressed.
        /// </summary>
        ENDCURSOR,

        /// <summary>
        /// Indicates moving a cursor one page up.  Also can be seen on input to indicate the
        /// PgUp key being pressed.
        /// </summary>
        PAGEUPCURSOR,

        /// <summary>
        /// Indicates moving a cursor one page down.  Also can be seen on input to indicate the
        /// PgDn key being pressed.
        /// </summary>
        PAGEDOWNCURSOR,

        /// <summary>
        /// Delete the character to the left of the cursor, and move the cursor left one
        /// space, wrapping to the end of the previous line if at the left edge of the screen.
        /// Also used for input to represent the keypress that does that (the backspace).
        /// </summary>
        DELETELEFT,
        
        /// <summary>
        /// Delete the character the cursor is currently on, and shift the characters to
        /// the right of it one space left to fill the gap.  (Does not wrap because the
        /// terminal doesn't know where the wraparound lines versus true end of lines are.)
        /// Also used for input to represent the keypress that does that (the delete button).
        /// </summary>
        DELETERIGHT,
        
        /// <summary>
        /// Abstracts away all that CR/LF vs LF only versus CR only nonsense.  In the pretend
        /// unicode terminal we are referring to, we'll map them all to the same character,
        /// this one.  This character means go to the start of the next line.
        /// Also used on input to represent hitting either the return or the enter key.
        /// </summary>
        NEWLINERETURN
    }

    // For tracking multiple-character input sequences to remember where it is in the sequence:
    public enum ExpectNextChar {
        NORMAL, RESIZEWIDTH, RESIZEHEIGHT, INTITLE,
    }

}
