import clr
from math import *
import sys
import time
sys.path.append("C:\\Users\\sunjiawei\\source\\repos\\BlokusGUI\\BlokusGUI")
from BlokusMod import SetInfo
from System import String
from System.Collections import *
from System.Drawing import *

def test(si):
    best = si
    best.Rotate = 4
    return f"cpu turn{best.Rotate}"
    

def CpuStep(client,board,game,rotateList):
    startTime = time.time()
    giveup = True
    pieceList = [19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 19, 18, 8, 7, 6, 5, 4, 17, 16, 3, 2, 1, 0]
    pre_score = -100
    jkjk = 0
    
    if client.IsMyChoice == False:
        return f"not my turn"
    for piece in pieceList:
        if game.Players[game.TurnPlayer].PiecesUsed[piece]:
            continue
        for x in range(board.BoardSize):
            for y in range(board.BoardSize):
                pos = Point(x,y)
                distance = sqrt(pow(board._startPoint[game.TurnPlayer].X-x,2)+pow(board._startPoint[game.TurnPlayer].Y-y,2))
                target = Point((int)(board.BoardSize/2), (int)(board.BoardSize/2))
                if distance > board.BoardSize/2 and distance < 1.6*board.BoardSize/2:
                    target = board._startPoint[game.TurnPlayer]
                elif distance < board.BoardSize:
                    target = Point(board.BoardSize-board._startPoint[game.TurnPlayer].X, board.BoardSize-board._startPoint[game.TurnPlayer].Y)
                for r in rotateList:
                    si = SetInfo(piece,r,pos)
                    if board.CheckPlace(game.Turn, si):
                        giveup = False
                        score = 0
                        score += 5 * (1 - sqrt(pow(target.X-x,2)+pow(target.Y-y,2)) / (sqrt(2) * board.BoardSize/2) ) # distance from target.
                        score += board.Pieces[piece].GetCellsNum()*1.3
                        blocked = []
                        for point in board.Pieces[piece].Cells(r):
                            p = Point(point.X + x, point.Y + y)
                            edges = [ Point(1, 0), Point(-1, 0), Point(0, 1), Point(0, -1) ]
                            corners = [ Point(1, 1), Point(-1, 1), Point(-1, -1), Point(1, -1) ]
                            block = False
                            color = -10
                            for corner in corners:
                                cx = p.X+corner.X
                                cy = p.Y+corner.Y
                                if cx >= 0 and cx < board.BoardSize and cy >= 0 and cy < board.BoardSize \
                                and board.Cell[cy, cx] != -1 and board.Cell[cy, cx] != game.TurnPlayer:
                                    color = board.Cell[cy, cx]
                                    edgeCount = 0
                                    if cx >=0 and cx < board.BoardSize and cy+1 >=0 and cy+1 < board.BoardSize and board.Cell[cy + 1, cx] == board.Cell[cy, cx]:
                                        edgeCount += 1
                                    if cx >=0 and cx < board.BoardSize and cy-1 >=0 and cy-1 < board.BoardSize and board.Cell[cy - 1, cx] == board.Cell[cy, cx]:
                                        edgeCount += 1
                                    if edgeCount >= 2:
                                        continue
                                    edgeCount = 0
                                    if cx+1 >=0 and cx+1 < board.BoardSize and cy >=0 and cy < board.BoardSize and board.Cell[cy, cx + 1] == board.Cell[cy, cx]:
                                        edgeCount += 1
                                    if cx-1 >=0 and cx-1 < board.BoardSize and cy >=0 and cy < board.BoardSize and board.Cell[cy, cx - 1] == board.Cell[cy, cx]:
                                        edgeCount += 1

                                    if edgeCount <= 1:
                                        block = True
                            for edge in edges:
                                cx = p.X+edge.X
                                cy = p.Y+edge.Y
                                if cx >=0 and cx < board.BoardSize and cy >=0 and cy < board.BoardSize and board.Cell[cy, cx] == color: # meaningless block.
                                    block = False
                                    break
                            if block:
                                blocked.append(p)
                        score += len(blocked)
                        if score > pre_score:
                            jkjk = len(blocked)
                            pre_score = score
                            best = SetInfo(piece,r,pos)
                            success = True
    endTime = time.time()
    if giveup == True:
        client.GiveUp()
        return f"{game.TurnPlayer} giveup"
    elif giveup == False:
        # set best piece.
        board.SetPiece(game.Turn,best)
        client.IsMyChoice = False
        game.SetPiece(best)
        client.SetPiece(best)
        return f"player{game.TurnPlayer} p:{best.Piece} r:{best.Rotate} x:{best.Cell.X} y:{best.Cell.Y} target:{endTime - startTime} placed len:{jkjk} score:{score}"