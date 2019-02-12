import json
import os

indir = "../Assets/StreamingAssets/3d/"
outdir = "../Assets/StreamingAssets/3d/"


def normalize(filename):

	with open(filename) as file:
		data = json.load(file)
		
		minX = float('inf')
		minY = float('inf')
		minZ = float('inf')

		maxX = float('-inf')
		maxY = float('-inf')
		maxZ = float('-inf')

		for n in data['nodes']:
			if float(n['x']) < minX: 
				minX = float(n['x'])
			if float(n['y']) < minY: 
				minY = float(n['y'])
			if float(n['z']) < minZ: 
				minZ = float(n['z'])

			if float(n['x']) > maxX: 
				maxX = float(n['x'])
			if float(n['y']) > maxY: 
				maxY = float(n['y'])
			if float(n['z']) > maxZ: 
				maxZ = float(n['z'])

		diffX = maxX - minX
		diffY = maxY - minY
		diffZ = maxZ - minZ

		maxDiff = max(diffX, diffY, diffZ)

		for n in data['nodes']:
			n['x'] = (((float(n['x']) - minX) / diffX * 2) - 1) * maxDiff / diffX
			n['y'] = (((float(n['y']) - minY) / diffY * 2) - 1) * maxDiff / diffY
			n['z'] = (((float(n['z']) - minZ) / diffZ * 2) - 1) * maxDiff / diffZ

		return json.dumps(data, indent=4, separators=(',', ': '))




#Script
files = list(filter(lambda s: s[len(s)-5:] == ".json", os.listdir(indir)))

for f in files:
	#Do the convert
	json_out = normalize(indir + f)
	#Save to outdir
	open(outdir + f, 'w').write(json_out)