import clr
from math import *
import copy
import sys
sys.path.append("C:\\Users\\sunjiawei\\source\\repos\\BlokusGUI\\BlokusGUI")
from BlokusMod import SetInfo
from System import String
from System.Collections import *
from System.Drawing import *

def test(si):
    best = si
    best.Rotate = 4
    return f"cpu turn{best.Rotate}"
    

def CpuStep(client,board,game,si,rotateList):
    giveup = True
    pieceList = [19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 19, 18, 8, 7, 6, 5, 4, 17, 16, 3, 2, 1, 0]
    pieceList.reverse()
    pre_score = 0
    success = False
    
    if client.IsMyChoice == False:
        return f"not my turn"
    for piece in pieceList:
        if game.Players[game.TurnPlayer].PiecesUsed[piece]:
            continue
        for x in range(board.BoardSize):
            for y in range(board.BoardSize):
                pos = Point(x,y)
                target = Point((int)(board.BoardSize/2),(int)(board.BoardSize/2))
                for r in rotateList:
                    si = SetInfo(piece,r,pos)
                    if board.CheckPlace(game.Turn, si):
                        giveup = False
                        score = 0
                        score += 5 * (1 - sqrt(pow(target.X-x,2)+pow(target.Y-y,2)) / (sqrt(2) * board.BoardSize/2) ) # distance from center.
                        score += board.Pieces[piece].GetCellsNum()
                        if score > pre_score:
                            pre_score = score
                            best = SetInfo(piece,r,pos)
                            success = True
                            
    if giveup == True:
        client.GiveUp()
        return f"si:{si.Piece} {si.Rotate} {si.Cell.X} {si.Cell.Y} giveup"
    elif giveup == False:
        # set best piece.
        board.SetPiece(game.Turn,best)
        client.IsMyChoice = False
        game.SetPiece(best)
        client.SetPiece(best)
        return f"p:{best.Piece} r:{best.Rotate} x:{best.Cell.X} y:{best.Cell.Y}  placed success?:{success} score:{score}"