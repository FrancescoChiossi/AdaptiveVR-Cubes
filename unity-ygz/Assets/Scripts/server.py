import socket
import sys
from ledapy.deconvolution import sdeconv_analysis
from numpy import array as npa
# import cvxEDA as cvx
import numpy as np
import neurokit2 as nk

# Create a TCP/IP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
# Bind the socket to the port
server_address = ('localhost', 8052)
print('starting up on %s port %s' % server_address)
sock.bind(server_address)

# Listen for incoming connections
sock.listen(1)

while True:
    # Wait for a connection
    print('waiting for a connection')
    connection, client_address = sock.accept()

    #tonic_old = []

    try:
        print(f'connection from {0}', client_address)

        # Receive the data in small chunks and retransmit it
        while True:
            data = connection.recv(1024).decode("utf-8")
            #print(len(data))
            if len(data) >= 1:
                data_splitted_list = data.split("_")
                data_float_list = []
                for data_str in data_splitted_list:
                    if data_str == "":
                        break
                    data_float_list.append(float(data_str))

                data_np = npa(data_float_list)
                print(data_np)

                # Process it
                signals, info = nk.eda_process(data_np, sampling_rate=1000)
                # Visualise the processing
                # nk.eda_plot(signals, sampling_rate=1000)
                # average = np.average(signals['EDA_Tonic'])

                signals['EDA_Tonic']
                signals_data = ""

                for signal in signals['EDA_Tonic']:
                    signals_data += str(signal) + "_"
                signals_data = signals_data[:len(signals_data)-1]

                connection.sendall(signals_data.encode("utf-8"))
            

            #print('received {0}'.format(data))
            #if data:
                #print('sending data back to the client')
                #connection.sendall(data)
            #else:
            #    print(f'no more data from {0}', client_address)
            #    break
            
    finally:
        # Clean up the connection
        connection.close()