import clr
from System import String
from System.Collections import *
from System.Drawing import Point

def check(_board,si):
    si.Piece = 10
    return _board.CheckPlace(0,si)

def cpuStep():
    piece = 0
    r = 0
    x = 0
    y = 0
    return [piece, r, x, y]