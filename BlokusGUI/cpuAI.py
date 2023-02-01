import clr
from math import *
import sys
import time
sys.path.append("C:\\Users\\sunjiawei\\source\\repos\\BlokusGUI\\BlokusGUI")
from BlokusMod import *
from System import String
from System.Collections import *
from System.Drawing import *
    

def CpuStep(client,board,game,rotateList):
    startTime = time.time()
    giveup = True
    pieceList = [19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 19, 18, 8, 7, 6, 5, 4, 17, 16, 3, 2, 1, 0]
    pre_score = -100
    len_blocked = 0
    len_search,search_result = 0,0

    if client.IsMyChoice == False:
        return f"not my turn"
    for piece in pieceList:
        if game.Players[game.TurnPlayer].PiecesUsed[piece]:
            continue
        for x in range(board.BoardSize):
            for y in range(board.BoardSize):
                pos = Point(x,y)
                distance = sqrt(pow(board._startPoint[game.TurnPlayer].X-x,2)+pow(board._startPoint[game.TurnPlayer].Y-y,2)) # distance from startpoint
                target = Point((int)(board.BoardSize/2), (int)(board.BoardSize/2)) # select the strategy according to the position
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
                        score += board.Pieces[piece].GetCellsNum()*1.3 # use big piece first
                        blocked = []
                        for point in board.Pieces[piece].Cells(r): # how many routes can be blocked. 
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
                                    edgeCount = 0 # do pieces around the cell is in opposite places?
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
                            for edge in edges: # does the cell is near by other players' pieces?
                                cx = p.X+edge.X
                                cy = p.Y+edge.Y
                                if cx >=0 and cx < board.BoardSize and cy >=0 and cy < board.BoardSize and board.Cell[cy, cx] == color: # meaningless block.
                                    block = False
                                    break
                            if block:
                                blocked.append(p)
                        score += len(blocked)
                        if game.Players[game.TurnPlayer].PiecesUsed.count(False) < 5: # only active when rest pieces are less than 5
                            search_result = search(game,board,rotateList,si) # maximum points search
                            score += search_result * 0.2
                        if score > pre_score:
                            len_blocked = len(blocked)
                            len_search = search_result
                            pre_score = score
                            best = SetInfo(piece,r,pos)
                            success = True
    endTime = time.time()
    if giveup == True:
        client.GiveUp()
        return f"PLAYER:{game.TurnPlayer} giveup"
    elif giveup == False:
        # set best piece.
        board.SetPiece(game.Turn,best)
        client.IsMyChoice = False
        game.SetPiece(best)
        client.SetPiece(best)
        return f"PLAYER{game.TurnPlayer}--p:{best.Piece} r:{best.Rotate} x:{best.Cell.X} y:{best.Cell.Y}--TIME:{endTime - startTime} Blocked:{len_blocked} Score:{score} ::{len_search} rest:{game.Players[game.TurnPlayer].PiecesUsed.count(False)}"
                                                                                                                        
def search(game,board,rotateList,sinfo):
    pieceUsed = game.Players[game.TurnPlayer].PiecesUsed
    this_board = Board()
    this_board.Initialize(board.BoardSize)
    this_board.SetPiece(game.Turn,sinfo)
    pieceUsed[sinfo.Piece] = True

    for x in range(this_board.BoardSize):
        for y in range(this_board.BoardSize):
             this_board.Cell[y,x] = board.Cell[y,x]
    limit = 0
    while True:
        limit += 1
        p = PlacePiece(game,this_board,rotateList,pieceUsed)
        if limit > 3 or p == -1: # reach the limit or have nowhere to place.
            break
        
        
    count = 0
    for x in range(this_board.BoardSize):
        for y in range(this_board.BoardSize):
             if this_board.Cell[y,x] == game.Turn and board.Cell[y,x] != game.Turn:
                count += 1
    
    return count


def PlacePiece(game,board,rotateList,pieceUsed):
    pieceList = [19, 18, 17, 16, 15, 14, 13, 12, 11, 10, 9, 19, 18, 8, 7, 6, 5, 4, 17, 16, 3, 2, 1, 0]
    for piece in pieceList:
        if pieceUsed[piece]:
            continue
        for x in range(board.BoardSize):
            for y in range(board.BoardSize):
                pos = Point(x,y)
                for r in rotateList:
                    si = SetInfo(piece,r,pos)
                    if board.CheckPlace(game.Turn,si):
                        board.SetPiece(game.Turn,si)
                        pieceUsed[piece] = True
                        return piece
    return -1