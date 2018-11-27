import os
import json

n = json.load(open('node/nodes.json','r'))

nodes = []
for node in n['nodes']:
	nodes.append({
		'id': node['idd'],
		'label': node['name'],
		'x': node['xx'],
		'y': node['yy'],
		'z': node['zz']
		})

l1 = json.load(open('link/a01_Day2_Post.json','r'))
l2 = json.load(open('link/a02_Day2_Post.json','r'))

w1 = [x['value'] for x in l1['links']]
w2 = [x['value'] for x in l2['links']]

wmin = min(min(w1), min(w2))
wmax = max(max(w1), max(w2))

w1 = [(x-wmin)/(wmax-wmin) for x in w1]
w2 = [(x-wmin)/(wmax-wmin) for x in w2]

ob1 = {'nodes': nodes, 'links':l1['links']}
ob2 = {'nodes': nodes, 'links':l2['links']}

for i in range(0, len(ob1['links'])):
	ob1['links'][i]['value'] = w1[i]
	ob2['links'][i]['value'] = w2[i]

with open('brain_stressed.json','w') as f1:
	json.dump(ob1, f1, indent=4)

with open('brain_control.json','w') as f2:
	json.dump(ob2, f2, indent=4)

